using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using VMFramework.Core;

namespace VMFramework.UI
{
    [RequireComponent(typeof(UIDocument))]
    [DisallowMultipleComponent]
    public class UIToolkitPanel : UIPanel, IUIToolkitPanel, IUIPanelPointerEventProvider, ILocalizedPanelModifier
    {
        [TitleGroup(ComponentNames.CONFIG)]
        public bool autoPanelSettings = true;

        public UIDocument UIDocument { get; private set; }

        protected IUIToolkitPanelConfig UIToolkitPanelConfig => (IUIToolkitPanelConfig)GamePrefab;

        private UIToolkitRootReleaseWork runtimeRoot;
        private Exception layoutRetirementFailure;

        public VisualElement RootVisualElement => runtimeRoot?.Root;

        public Task LayoutCompletion { get; private set; }

        public CancellationToken LayoutCancellationToken { get; private set; }

        public event Action<IUIToolkitPanel> OnLayoutChangeEvent;

        public event Action<IUIToolkitPanel, VisualElement> OnRootVisualElementReady;

        public event Action<IUIToolkitPanel, UIToolkitRootRetirement> OnRootVisualElementReleased;

        public event IUIToolkitPanel.GenerateVisualElementHandler OnGenerateVisualElement;

        protected CancellationTokenSource OpenCTS => runtimeRoot?.Cancellation;

        public BindVisualElementsManager BindVisualElementsManager { get; protected set; }

        #region Pool Events

        protected override void OnCreate()
        {
            BindVisualElementsManager = GetComponentInChildren<BindVisualElementsManager>();

            base.OnCreate();

            var uiDocument = GetComponent<UIDocument>();

            if (autoPanelSettings)
            {
                uiDocument.panelSettings = UIToolkitPanelConfig.PanelSettings;
            }

            var visualTree = UIToolkitPanelConfig.VisualTree;
            if (uiDocument.visualTreeAsset != visualTree) uiDocument.visualTreeAsset = visualTree;

            UIDocument = uiDocument;
            ApplyClosedDocumentState();
        }

        protected virtual void OnEnable()
        {
            // Unity enables this companion before the pooled item is initialized,
            // and again after Live Reload has replaced the document's root.
            if (UIDocument == null) return;
            if (IsOpened)
            {
                try { PublishRuntimeRoot(); }
                catch (Exception primary)
                {
                    var failures = new UIPanelReleaseFailures(nameof(UIToolkitPanel), primary);
                    failures.Attempt("enabled-root.retire", () => UIPanelManager.Instance.RetireFailedOpen(this, primary));
                    failures.ThrowIfFailed(); throw;
                }
            }
            else ApplyClosedDocumentState();
        }

        protected virtual void OnDisable() => ReleaseRuntimeRoot();

        protected override void OnClear()
        {
            ReleaseRuntimeRoot(afterLayout: () => base.OnClear());
        }

        protected override void OnDestroy()
        {
            var failures = new UIPanelReleaseFailures(nameof(UIToolkitPanel));
            failures.Attempt("native-destruction.close", BeginPanelDestruction);
            failures.Attempt("native-destruction.root", () => ReleaseRuntimeRoot(
                failures.PrimaryFailure, CompletePanelDestruction));
            failures.ThrowIfFailed();
        }

        #endregion

        #region Open

        protected override void OnOpenInternal(IUIPanel source)
        {
            base.OnOpenInternal(source);

            if (UIDocument.enabled == false)
            {
                UIDocument.enabled = true;
            }

            PublishRuntimeRoot();
        }

        private void PublishRuntimeRoot()
        {
            if (runtimeRoot != null)
                throw new InvalidOperationException($"UIToolkitPanel {name} already owns a published runtime root.");

            var work = new UIToolkitRootReleaseWork(UIDocument.rootVisualElement);
            runtimeRoot = work;
            LayoutCompletion = null;
            layoutRetirementFailure = null;

            RootVisualElement.DisplayFlex();

            RootVisualElement.style.visibility = Visibility.Hidden;

            work.Cancellation = new();
            work.Token = work.Cancellation.Token;
            LayoutCancellationToken = work.Token;

            OnGenerateVisualElement?.Invoke(this, RootVisualElement);

            OnRootVisualElementReady?.Invoke(this, RootVisualElement);

            if (lastLocale != null) OnCurrentLanguageChanged(lastLocale);
            if (OnPointerEnterEvent != null || OnPointerLeaveEvent != null)
                RegisterPointerEvents(work);

            var actual = CompleteLayoutAsync(work.Token);
            work.Layout = actual;
            LayoutCompletion = actual;
            var awaiter = actual.GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                try { awaiter.GetResult(); }
                catch (OperationCanceledException error) when
                    (error.CancellationToken == work.Token && work.Token.IsCancellationRequested) { }
                catch (Exception error)
                {
                    var failures = new UIPanelReleaseFailures(nameof(UIToolkitPanel), error);
                    if (ReferenceEquals(runtimeRoot, work) && work.Retirement == null)
                        failures.Attempt("layout-failure.retire", () => UIPanelManager.Instance.RetireFailedOpen(this, error));
                    try { failures.ThrowIfFailed(); }
                    catch (Exception primary) { Debug.LogException(primary, this); }
                }
                finally
                {
                    try { work.Retirement?.Invoke(work); }
                    catch (Exception error) { Debug.LogException(error, this); }
                }
            });
        }

        private async Task CompleteLayoutAsync(CancellationToken token)
        {
            await UniTask.Yield(token, cancelImmediately: true);
            token.ThrowIfCancellationRequested();
            OnLayoutChange();
            token.ThrowIfCancellationRequested();
            OnPostLayoutChange();
            token.ThrowIfCancellationRequested();
            OnLayoutChangeEvent?.Invoke(this);
        }

        private void CancelLayout()
        {
            runtimeRoot?.CancelLayout();
        }

        private void ReleaseRuntimeRoot(Exception failure = null, Action afterLayout = null)
        {
            var work = runtimeRoot;
            if (work == null) { afterLayout?.Invoke(); return; }
            if (work.Root == null) { afterLayout?.Invoke(); return; }
            work.SourceFailure ??= failure;
            work.Retirement ??= CompleteRequestedRootRetirement;
            work.AfterLayout += afterLayout;
            var failures = new UIPanelReleaseFailures(nameof(UIToolkitPanel), failure);
            failures.Attempt("layout.cancel-and-dispose", work.CancelLayout);
            if (work.Layout == null || work.Layout.IsCompleted)
                failures.Attempt("root.retire", () => work.Retirement?.Invoke(work));
            failures.ThrowIfReleaseFailed();
        }

        private void CompleteRequestedRootRetirement(UIToolkitRootReleaseWork work)
        {
            work.Retirement = null;
            var released = work.Root; work.Root = null;
            var afterLayout = work.AfterLayout; work.AfterLayout = null;
            var failures = new UIPanelReleaseFailures(nameof(UIToolkitPanel), work.SourceFailure);
            failures.Attempt("pointer.unsubscribe", work.ReleasePointer);
            if (released != null)
            {
                var product = new UIToolkitRootRetirement(released, failures.PrimaryFailure, work.Layout);
                var subscribers = OnRootVisualElementReleased;
                if (subscribers != null)
                    foreach (Action<IUIToolkitPanel, UIToolkitRootRetirement> subscriber in subscribers.GetInvocationList())
                        try { subscriber(this, product); }
                        catch (Exception error) { failures.Capture("root-released.subscriber", error); }
            }
            if (afterLayout != null)
                foreach (Action release in afterLayout.GetInvocationList())
                    failures.Attempt("after-layout.release", release);
            runtimeRoot = null;
            if (failures.HasReleaseFailures) layoutRetirementFailure = failures.PrimaryFailure;
            failures.ThrowIfReleaseFailed();
        }

        void IUIToolkitPanel.CompleteLayoutRetirement(Task actualLayout)
        {
            var work = runtimeRoot;
            if (work != null)
            {
                if (!ReferenceEquals(work.Layout, actualLayout))
                    throw new InvalidOperationException($"UIToolkitPanel {id} retirement does not own this layout Task.");
                try { work.Retirement?.Invoke(work); }
                catch (Exception failure) when (ReferenceEquals(failure, layoutRetirementFailure)) { }
            }
            var actualFailure = layoutRetirementFailure; layoutRetirementFailure = null;
            if (actualFailure != null) ExceptionDispatchInfo.Capture(actualFailure).Throw();
        }

        #endregion

        #region Close

        protected override void OnPreCloseInternal()
        {
            var failures = new UIPanelReleaseFailures(nameof(UIToolkitPanel));
            failures.Attempt("base.pre-close", () => base.OnPreCloseInternal());
            failures.Attempt("layout.cancel", CancelLayout);
            failures.ThrowIfFailed();
        }

        protected override void OnPostCloseInternal(Exception failure)
        {
            var failures = new UIPanelReleaseFailures(nameof(UIToolkitPanel), failure);
            failures.Attempt("base.post-close", () => base.OnPostCloseInternal(failure));
            failures.Attempt("root.release", () => ReleaseRuntimeRoot(failure, ApplyClosedDocumentState));
            failures.ThrowIfReleaseFailed();
        }

        private void ApplyClosedDocumentState()
        {
            // Rental/native retirement has already consumed this document's lifetime.
            if (IsDestroyed) return;
            if (UIToolkitPanelConfig.CloseMode == UIToolkitPanelCloseMode.DisableDocument)
            {
                UIDocument.enabled = false;
            }
            else
            {
                UIDocument.enabled = true;
                UIDocument.rootVisualElement.DisplayNone();
            }
        }

        #endregion

        #region Layout Change

        protected virtual void OnLayoutChange()
        {

        }

        protected virtual void OnPostLayoutChange()
        {
            RootVisualElement.style.visibility = Visibility.Visible;

            if (UIToolkitPanelConfig.IgnoreMouseEvents)
            {
                RootVisualElement.SetPickingMode(PickingMode.Ignore);
            }
        }

        #endregion

        public virtual VisualElement GenerateVisualElement()
        {
            var uiDocument = GetComponent<UIDocument>();
            var root = uiDocument.visualTreeAsset.CloneTree();
            OnGenerateVisualElement?.Invoke(this, root);
            return root;
        }

        private Action<IUIPanel> OnPointerEnterEvent;
        private Action<IUIPanel> OnPointerLeaveEvent;

        void IUIPanelPointerEventProvider.AddPointerEvent(Action<IUIPanel> onPointerEnter,
            Action<IUIPanel> onPointerLeave)
        {
            OnPointerEnterEvent = onPointerEnter;
            OnPointerLeaveEvent = onPointerLeave;

            if (runtimeRoot != null) RegisterPointerEvents(runtimeRoot);
        }

        private void RegisterPointerEvents(UIToolkitRootReleaseWork work)
        {
            if (work.Pointer != null)
                throw new InvalidOperationException($"UIToolkitPanel {id} already owns pointer endpoints for this root.");
            work.Pointer = new UIToolkitPointerEndpoints(OnPointerEnter, OnPointerLeave);
            try { work.Pointer.Acquire(work.Root); }
            catch (Exception primary)
            {
                var failures = new UIPanelReleaseFailures(nameof(UIToolkitPanel), primary);
                failures.Attempt("partial-pointer.unsubscribe", work.ReleasePointer);
                failures.ThrowIfFailed(); throw;
            }
        }

        void IUIPanelPointerEventProvider.RemovePointerEvent()
        {
            OnPointerEnterEvent = null;
            OnPointerLeaveEvent = null;
            runtimeRoot?.ReleasePointer();
        }

        private void OnPointerEnter(MouseEnterEvent e)
        {
            OnPointerEnterEvent?.Invoke(this);
        }

        private void OnPointerLeave(MouseLeaveEvent e)
        {
            OnPointerLeaveEvent?.Invoke(this);
        }

        protected Locale lastLocale { get; private set; }

        void ILocalizedPanelModifier.OnCurrentLanguageChanged(Locale currentLocale)
        {
            OnCurrentLanguageChanged(currentLocale);
        }

        protected virtual void OnCurrentLanguageChanged(Locale currentLocale)
        {
            if (UISetting.UIPanelGeneralSetting.enableLanguageConfigs == false)
            {
                return;
            }

            if (lastLocale != null)
            {
                var previousLanguageConfig =
                    UISetting.UIPanelGeneralSetting.GetLanguageConfig(lastLocale.Identifier.Code);

                if (previousLanguageConfig != null)
                {
                    RootVisualElement.styleSheets.Remove(previousLanguageConfig.styleSheet);
                }
            }

            lastLocale = currentLocale;

            var currentLanguageConfig =
                UISetting.UIPanelGeneralSetting.GetLanguageConfig(currentLocale.Identifier.Code);

            if (currentLanguageConfig != null)
            {
                RootVisualElement.styleSheets.Add(currentLanguageConfig.styleSheet);
            }
        }
    }
}
