using System;
using System.Collections.Generic;
using VMFramework.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core.Linq;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public abstract partial class UIPanel : ControllerGameItem, IUIPanel
    {
        [ShowInInspector]
        public IUIPanelConfig Config => (IUIPanelConfig)GamePrefab;
        
        public bool IsUnique => Config.IsUnique;
        
        [ShowInInspector]
        public bool UIEnabled { get; private set; }

        [ShowInInspector]
        public bool IsOpened { get; protected set; } = false;

        [ShowInInspector]
        public IUIPanel SourceUIPanel { get; private set; }

        public IReadOnlyCollection<IPanelModifier> Modifiers => modifiers;

        [ShowInInspector, HideInEditorMode]
        private List<IPanelModifier> modifiers;
        
        public event Action<IUIPanel> OnOpenEvent;
        
        public event Action<IUIPanel> OnDestructEvent;
        
        public event Action<IUIPanel> OnPreCloseEvent;

        public event Action<IUIPanel> OnPostCloseEvent;

        #region Pool Events

        protected override void OnCreate()
        {
            base.OnCreate();
            
            UIPanelManager.Register(this);

            if (Config.ModifiersConfigs.IsNullOrEmpty())
            {
                return;
            }

            modifiers = ListPool<IPanelModifier>.Default.Get();
            modifiers.Clear();

            foreach (var processorConfig in Config.ModifiersConfigs)
            {
                var processor = GameItemManager.Get<IPanelModifier>(processorConfig.id);
                modifiers.Add(processor);
            }

            foreach (var processor in modifiers)
            {
                processor.transform.SetParent(transform);
                processor.Initialize(this, Config);
            }
        }

        protected override void OnClear()
        {
            base.OnClear();

            if (modifiers.IsNullOrEmpty() == false)
            {
                foreach (var processor in modifiers)
                {
                    GameItemManager.Return(processor);
                }
                
                modifiers.Clear();
                
                modifiers.ReturnToDefaultPool();
            }
            
            UIPanelManager.Unregister(this);
            
            OnDestructEvent?.Invoke(this);
        }

        #endregion

        #region Basic Event

        protected virtual void OnSetEnabled()
        {

        }

        #endregion

        #region Open

        void IUIPanel.OnOpen(IUIPanel source)
        {
            if (IsOpened)
            {
                return;
            }
            
            OnOpen(source);

            SourceUIPanel = source;

            if (SourceUIPanel != null)
            {
                SourceUIPanel.OnPostCloseEvent += UIPanelManager.Close;
            }
            
            IsOpened = true;
            
            OnPostOpen(source);
            
            OnOpenEvent?.Invoke(this);
        }

        protected virtual void OnOpen(IUIPanel source)
        {
            
        }

        protected virtual void OnPostOpen(IUIPanel source)
        {
            
        }

        #endregion

        #region Close

        protected virtual void OnClose()
        {
            OnPreCloseEvent?.Invoke(this);
            
            IsOpened = false;
        }
        
        protected virtual void OnPostClose()
        {
            OnPostCloseEvent?.Invoke(this);
            
            if (SourceUIPanel != null)
            {
                SourceUIPanel.OnPostCloseEvent -= UIPanelManager.Close;
            }

            SourceUIPanel = null;
        }

        void IUIPanel.OnClose()
        {
            OnClose();
        }

        void IUIPanel.OnPostClose()
        {
            OnPostClose();
        }

        #endregion
        
        public void SetEnabled(bool enableState)
        {
            if (IsDebugging)
            {
                Debugger.LogWarning($"Trying to set enabled state of {name} to {enableState}");
            }

            UIEnabled = enableState;

            OnSetEnabled();
        }
    }
}