using System.Collections.Generic;
using VMFramework.Core;
using Sirenix.OdinInspector;
using VMFramework.Core.Linq;
using VMFramework.GameLogicArchitecture;
using VMFramework.Timers;

namespace VMFramework.UI
{
    public partial class UIPanel : ControllerGameItem, IUIPanel
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

        #region Pool Events

        protected override void OnCreate()
        {
            base.OnCreate();

            BindObjectsManager = GetComponentInChildren<BindObjectsManager>();

            modifiers.Clear();

            foreach (var modifier in GetComponentsInChildren<IPanelModifier>())
            {
                modifiers.Add(modifier);
            }
            
            modifiers.Sort(modifier => modifier.InitializePriority);

            UIPanelManager.Instance.Register(this);
            
            foreach (var modifier in modifiers)
            {
                modifier.Initialize(this, Config);
            }
        }

        protected override void OnClear()
        {
            base.OnClear();
            
            if (closableCheckAdded)
            {
                UpdateDelegateManager.Instance.OnUpdateEvent -= OnCheckingClosable;
                closableCheckAdded = false;
            }

            if (modifiers.Count > 0)
            {
                foreach (var modifier in modifiers)
                {
                    modifier.Deinitialize();
                }
                
                modifiers.Clear();
            }
            
            UIPanelManager.Instance.Unregister(this);
            
            OnDestruct?.Invoke(this);
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
            if (IsOpened)
            {
                return;
            }

            IsClosing = false;
            
            OnOpenInternal(source);

            SourceUIPanel = source;

            if (SourceUIPanel != null)
            {
                SourceUIPanel.OnPostClose += OnSourcePanelClose;
            }
            
            IsOpened = true;
            
            OnPostOpenInternal(source);
            
            OnOpen?.Invoke(this);
        }

        protected virtual void OnOpenInternal(IUIPanel source)
        {
            if (closableCheckAdded)
            {
                UpdateDelegateManager.Instance.OnUpdateEvent -= OnCheckingClosable;
                closableCheckAdded = false;
            }
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

        protected virtual void OnPostCloseInternal()
        {
            if (closableCheckAdded)
            {
                UpdateDelegateManager.Instance.OnUpdateEvent -= OnCheckingClosable;
                closableCheckAdded = false;
            }
            
            if (SourceUIPanel != null)
            {
                SourceUIPanel.OnPostClose -= OnSourcePanelClose;
            }

            SourceUIPanel = null;
        }

        void IUIPanel.OnPreCloseInternal()
        {
            OnPreClose?.Invoke(this);
            OnPreCloseInternal();
        }

        void IUIPanel.OnPostCloseInternal()
        {
            IsClosing = false;
            OnPostClose?.Invoke(this);
            OnPostCloseInternal();
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
    }
}