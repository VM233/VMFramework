using System;
using System.Threading;
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

        public VisualElement RootVisualElement { get; private set; }

        public event Action<IUIToolkitPanel> OnLayoutChangeEvent;

        public event Action<IUIToolkitPanel, VisualElement> OnRootVisualElementReady;

        public event Action<IUIToolkitPanel, VisualElement> OnRootVisualElementReleased;

        public event IUIToolkitPanel.GenerateVisualElementHandler OnGenerateVisualElement;

        protected CancellationTokenSource OpenCTS { get; private set; }

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

            uiDocument.visualTreeAsset = UIToolkitPanelConfig.VisualTree;

            uiDocument.enabled = false;

            UIDocument = uiDocument;
        }

        protected virtual void OnEnable()
        {
            // Unity enables this companion before the pooled item is initialized,
            // and again after Live Reload has replaced the document's root.
            if (UIDocument == null) return;
            if (IsOpened) PublishRuntimeRoot();
            else ApplyClosedDocumentState();
        }

        protected virtual void OnDisable() => ReleaseRuntimeRoot();

        protected override void OnClear()
        {
            ReleaseRuntimeRoot();
            base.OnClear();
        }

        protected override void OnDestroy()
        {
            ReleaseRuntimeRoot();
            base.OnDestroy();
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
            if (RootVisualElement != null)
                throw new InvalidOperationException($"UIToolkitPanel {name} already owns a published runtime root.");

            RootVisualElement = UIDocument.rootVisualElement;

            RootVisualElement.DisplayFlex();

            RootVisualElement.style.visibility = Visibility.Hidden;

            OpenCTS = new();

            OnGenerateVisualElement?.Invoke(this, RootVisualElement);

            OnRootVisualElementReady?.Invoke(this, RootVisualElement);

            if (lastLocale != null) OnCurrentLanguageChanged(lastLocale);
            if (OnPointerEnterEvent != null || OnPointerLeaveEvent != null)
                RegisterPointerEvents(RootVisualElement);

            CompleteLayoutAsync(OpenCTS.Token);
        }

        private async void CompleteLayoutAsync(CancellationToken token)
        {
            try
            {
                await UniTask.Yield(token);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            OnLayoutChange();

            OnPostLayoutChange();

            OnLayoutChangeEvent?.Invoke(this);
        }

        private void CancelLayout()
        {
            if (OpenCTS == null) return;
            OpenCTS.Cancel();
            OpenCTS.Dispose();
            OpenCTS = null;
        }

        private void ReleaseRuntimeRoot()
        {
            CancelLayout();
            if (RootVisualElement == null) return;

            var released = RootVisualElement;
            UnregisterPointerEvents(released);
            RootVisualElement = null;
            OnRootVisualElementReleased?.Invoke(this, released);
        }

        #endregion

        #region Close

        protected override void OnPreCloseInternal()
        {
            base.OnPreCloseInternal();

            CancelLayout();
        }

        protected override void OnPostCloseInternal()
        {
            base.OnPostCloseInternal();

            ReleaseRuntimeRoot();
            ApplyClosedDocumentState();
        }

        private void ApplyClosedDocumentState()
        {
            if (UIToolkitPanelConfig.CloseMode == UIToolkitPanelCloseMode.DisableDocument)
            {
                UIDocument.enabled = false;
            }
            else if (UIDocument.enabled)
            {
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

            if (RootVisualElement != null) RegisterPointerEvents(RootVisualElement);
        }

        private void RegisterPointerEvents(VisualElement root)
        {
            foreach (var visualElement in root.Children())
            {
                visualElement.RegisterCallback<MouseEnterEvent>(OnPointerEnter);
                visualElement.RegisterCallback<MouseLeaveEvent>(OnPointerLeave);
            }
        }

        void IUIPanelPointerEventProvider.RemovePointerEvent()
        {
            if (RootVisualElement != null) UnregisterPointerEvents(RootVisualElement);
            OnPointerEnterEvent = null;
            OnPointerLeaveEvent = null;
        }

        private void UnregisterPointerEvents(VisualElement root)
        {
            foreach (var visualElement in root.Children())
            {
                visualElement.UnregisterCallback<MouseEnterEvent>(OnPointerEnter);
                visualElement.UnregisterCallback<MouseLeaveEvent>(OnPointerLeave);
            }
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
