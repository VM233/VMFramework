using System;
using System.Collections.Generic;
using VMFramework.Core;
using Sirenix.OdinInspector;
using VMFramework.Core.Linq;
using VMFramework.GameLogicArchitecture;
using VMFramework.Timers;

namespace VMFramework.UI
{
    public class UIPanel : ControllerGameItem, IUIPanel
    {
        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        public IUIPanelConfig Config => (IUIPanelConfig)GamePrefab;
        
        public bool IsUnique => Config.IsUnique;
        
        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        public bool UIEnabled { get; private set; }

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        public bool IsOpened { get; protected set; } = false;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        public bool IsClosing { get; protected set; } = false;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector]
        public IUIPanel SourceUIPanel { get; private set; }

        public BindObjectsManager BindObjectsManager { get; protected set; }

        public IReadOnlyCollection<IPanelModifier> Modifiers => modifiers;

        public event IUIPanel.OpenHandler OnOpen;

        public event IUIPanel.ReenterHandler OnReenter;

        public event IUIPanel.CloseHandler OnPreClose;

        public event IUIPanel.CloseHandler OnPostClose;

        public event IUIPanel.ClosableCheckHandler OnCheckClosable;

        public event IUIPanel.DestructHandler OnDestruct;

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector, HideInEditorMode]
        protected readonly List<IPanelModifier> modifiers = new();
        
        protected bool closableCheckAdded = false;

        private UIPanelManager registration;

        #region Pool Events

        protected override void OnCreate()
        {
            base.OnCreate();
            BindObjectsManager = GetComponentInChildren<BindObjectsManager>();
            var ordered = new List<IPanelModifier>(GetComponentsInChildren<IPanelModifier>());
            ordered.Sort(modifier => modifier.InitializePriority);
            try
            {
                foreach (var modifier in ordered)
                {
                    modifier.Initialize(this, Config);
                    try { modifiers.Add(modifier); }
                    catch (Exception primary)
                    {
                        var failures = new UIPanelReleaseFailures(nameof(UIPanel), primary);
                        failures.Attempt("unadopted-modifier.deinitialize", modifier.Deinitialize);
                        failures.ThrowIfFailed(); throw;
                    }
                }
            }
            catch (Exception primary)
            {
                var failures = new UIPanelReleaseFailures(nameof(UIPanel), primary);
                failures.Attempt("modifiers.deinitialize", DeinitializeModifiers);
                failures.Attempt("panel.unregister", ReleaseRegistration);
                failures.ThrowIfFailed(); throw;
            }
        }

        protected override void OnReturn()
        {
            IsOpened = false; IsClosing = false;
            var failures = new UIPanelReleaseFailures(nameof(UIPanel));
            failures.Attempt("rental.resources", ReleasePanelCloseSubscriptions);
            failures.Attempt("rental.registration", ReleaseRegistration);
            failures.Attempt("base.return", () => base.OnReturn());
            failures.ThrowIfFailed();
        }

        protected override void OnClear()
        {
            var failures = new UIPanelReleaseFailures(nameof(UIPanel));
            failures.Attempt("base.clear", () => base.OnClear());
            failures.Attempt("close-subscriptions.release", ReleasePanelCloseSubscriptions);
            failures.Attempt("close-check.unsubscribe", ReleaseClosableCheck);
            failures.Attempt("modifiers.deinitialize", DeinitializeModifiers);
            failures.Attempt("panel.unregister", ReleaseRegistration);
            PublishDestruct(ref failures);
            failures.Attempt("native-panel.destroy", () => UnityEngine.Object.Destroy(gameObject));
            failures.ThrowIfFailed();
        }

        protected override void OnDestroy()
        {
            var failures = new UIPanelReleaseFailures(nameof(UIPanel));
            failures.Attempt("native-destruction.close", BeginPanelDestruction);
            failures.Attempt("native-destruction.resources", CompletePanelDestruction);
            failures.ThrowIfFailed();
        }

        protected void BeginPanelDestruction()
        {
            base.OnDestroy();
            var owner = registration;
            if (!ReferenceEquals(owner, null)) owner.RetireDestroyedPanel(this);
        }

        protected void CompletePanelDestruction()
        {
            var failures = new UIPanelReleaseFailures(nameof(UIPanel));
            failures.Attempt("close-subscriptions.release", ReleasePanelCloseSubscriptions);
            failures.Attempt("close-check.unsubscribe", ReleaseClosableCheck);
            failures.Attempt("modifiers.deinitialize", DeinitializeModifiers);
            failures.Attempt("panel.unregister", ReleaseRegistration);
            PublishDestruct(ref failures);
            failures.ThrowIfFailed();
        }

        private void PublishDestruct(ref UIPanelReleaseFailures failures)
        {
            var subscribers = OnDestruct; OnDestruct = null;
            if (subscribers != null)
                foreach (IUIPanel.DestructHandler subscriber in subscribers.GetInvocationList())
                    try { subscriber(this); }
                    catch (Exception error) { failures.Capture("destruct.subscriber", error); }
        }

        private void ReleaseRegistration()
        {
            var actual = registration;
            if (!ReferenceEquals(actual, null)) actual.Unregister(this);
        }

        UIPanelManager IUIPanel.RegistrationOwner => registration;

        void IUIPanel.AdoptRegistration(UIPanelManager owner)
        {
            if (!ReferenceEquals(registration, null))
                throw new InvalidOperationException($"Panel {id} already owns its registration.");
            registration = owner;
        }

        void IUIPanel.WithdrawRegistration(UIPanelManager owner)
        {
            if (!ReferenceEquals(registration, owner))
                throw new InvalidOperationException($"Panel {id} registration is owned by a different manager.");
            registration = null;
        }

        void IUIPanel.PublishDestroyedClose(bool preClosePublished)
        {
            IsOpened = false; IsClosing = false;
            var failures = new UIPanelReleaseFailures(nameof(UIPanel));
            if (!preClosePublished) PublishClose(OnPreClose, "native-destruction.pre-close", ref failures);
            PublishClose(OnPostClose, "native-destruction.post-close", ref failures);
            failures.Attempt("native-destruction.close-subscriptions", ReleasePanelCloseSubscriptions);
            failures.ThrowIfFailed();
        }

        private void ReleaseClosableCheck()
        {
            if (!closableCheckAdded) return;
            closableCheckAdded = false;
            UpdateDelegateManager.Instance.OnUpdateEvent -= OnCheckingClosable;
        }

        private void DeinitializeModifiers()
        {
            var actual = modifiers.ToArray(); modifiers.Clear();
            var failures = new UIPanelReleaseFailures(nameof(UIPanel));
            foreach (var modifier in actual)
                failures.Attempt("modifier.deinitialize", modifier.Deinitialize);
            failures.ThrowIfFailed();
        }

        #endregion

        #region Basic Event

        protected virtual void OnSetEnabled()
        {

        }

        #endregion

        #region Open

        void IUIPanel.OnOpenInternal(IUIPanel source)
        {
            if (IsOpened) return;
            IsClosing = false;
            OnOpenInternal(source);
            if (source != null)
            {
                source.OnPostClose += OnSourcePanelClose;
                SourceUIPanel = source;
            }
            IsOpened = true;
            OnPostOpenInternal(source);
            OnOpen?.Invoke(this);
        }

        protected virtual void OnOpenInternal(IUIPanel source)
        {
            ReleaseClosableCheck();
        }

        protected virtual void OnPostOpenInternal(IUIPanel source)
        {
            
        }

        public virtual void Reenter()
        {
            if (IsOpened == false)
            {
                return;
            }
            
            OnReenter?.Invoke(this);
        }

        #endregion

        #region Close

        protected virtual void OnPreCloseInternal()
        {
            IsClosing = true;
            var closable = true;
            OnCheckClosable?.Invoke(this, ref closable);
            if (closable)
            {
                IsOpened = false;
                IsClosing = false;
            }
            else
            {
                UpdateDelegateManager.Instance.OnUpdateEvent += OnCheckingClosable;
                closableCheckAdded = true;
            }
        }

        protected virtual void OnCheckingClosable()
        {
            var closable = true;
            OnCheckClosable?.Invoke(this, ref closable);
            if (closable)
            {
                IsOpened = false;
                IsClosing = false;
            }
        }

        protected virtual void OnPostCloseInternal(Exception failure)
        {
            ReleasePanelCloseSubscriptions();
        }

        private void ReleasePanelCloseSubscriptions()
        {
            var failures = new UIPanelReleaseFailures(nameof(UIPanel));
            failures.Attempt("close-check.unsubscribe", ReleaseClosableCheck);
            var source = SourceUIPanel; SourceUIPanel = null;
            if (source != null)
                failures.Attempt("source-close.unsubscribe", () => source.OnPostClose -= OnSourcePanelClose);
            failures.ThrowIfReleaseFailed();
        }

        void IUIPanel.OnPreCloseInternal()
        {
            var failures = new UIPanelReleaseFailures(nameof(UIPanel));
            PublishClose(OnPreClose, "pre-close.subscriber", ref failures);
            failures.Attempt("pre-close.internal", OnPreCloseInternal);
            failures.ThrowIfFailed();
        }

        void IUIPanel.OnPostCloseInternal()
        {
            IsOpened = false; IsClosing = false;
            var failures = new UIPanelReleaseFailures(nameof(UIPanel));
            PublishClose(OnPostClose, "post-close.subscriber", ref failures);
            var failure = failures.PrimaryFailure;
            failures.Attempt("post-close.internal", () => OnPostCloseInternal(failure));
            failures.ThrowIfFailed();
        }

        void IUIPanel.RetireFailedOpen(Exception failure)
        {
            bool adopted = IsOpened;
            IsOpened = false; IsClosing = false;
            var failures = new UIPanelReleaseFailures(nameof(UIPanel), failure);
            if (adopted)
            {
                PublishClose(OnPreClose, "failed-open.pre-close", ref failures);
                PublishClose(OnPostClose, "failed-open.post-close", ref failures);
            }
            failures.Attempt("failed-open.resources", () => OnPostCloseInternal(failure));
            failures.ThrowIfReleaseFailed();
        }

        void IUIPanel.RetireFailedClose(Exception failure)
        {
            IsOpened = false; IsClosing = false;
            var failures = new UIPanelReleaseFailures(nameof(UIPanel), failure);
            PublishClose(OnPostClose, "failed-close.post-close", ref failures);
            failures.Attempt("failed-close.resources", () => OnPostCloseInternal(failure));
            failures.ThrowIfReleaseFailed();
        }

        private void PublishClose(IUIPanel.CloseHandler subscribers, string stage, ref UIPanelReleaseFailures failures)
        {
            if (subscribers == null) return;
            foreach (IUIPanel.CloseHandler subscriber in subscribers.GetInvocationList())
                try { subscriber(this); }
                catch (Exception error) { failures.Capture(stage, error); }
        }

        #endregion
        
        protected virtual void OnSourcePanelClose(IUIPanel panel)
        {
            this.Close();
        }
        
        public void SetEnabled(bool enableState)
        {
            if (IsDebugging)
            {
                UnityEngine.Debug.LogWarning($"Trying to set enabled state of {name} to {enableState}");
            }

            UIEnabled = enableState;

            OnSetEnabled();
        }

#if UNITY_EDITOR
        [TitleGroup(ComponentNames.RUNTIME)]
        [Button]
        private void _Toggle() => this.Toggle();

        [TitleGroup(ComponentNames.RUNTIME)]
        [Button]
        private void _Open() => this.Open(null);

        [TitleGroup(ComponentNames.RUNTIME)]
        [Button]
        private void _Close() => this.Close();
#endif
    }
}
