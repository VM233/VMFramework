using System;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Procedure;
using VMFramework.Timers;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public class UIPanelPointerEventManager : ManagerBehaviour<UIPanelPointerEventManager>
    {
        [SerializeField]
        private bool isDebugging;

        #region PanelOnMouseHover

        private static IUIPanel _panelOnMouseHover;

        [ShowInInspector]
        protected static IUIPanel panelOnMouseHover
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _panelOnMouseHover;
            private set
            {
                var oldPanelOnMouseHover = _panelOnMouseHover;
                _panelOnMouseHover = value;
                OnPanelOnMouseHoverChanged?.Invoke(oldPanelOnMouseHover, _panelOnMouseHover);
            }
        }

        protected static event Action<IUIPanel, IUIPanel> OnPanelOnMouseHoverChanged;

        #endregion

        #region PanelOnMouseClick

        private static IUIPanel _panelOnMouseClick;

        [ShowInInspector]
        protected static IUIPanel panelOnMouseClick
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _panelOnMouseClick;
            private set
            {
                var oldPanelOnMouseClick = _panelOnMouseClick;
                _panelOnMouseClick = value;
                OnPanelOnMouseClickChanged?.Invoke(oldPanelOnMouseClick, _panelOnMouseClick);
            }
        }

        public static event Action<IUIPanel, IUIPanel> OnPanelOnMouseClickChanged;

        #endregion
        
        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();
            
            UIPanelManager.OnPanelCreatedEvent += OnPanelCreated;
            
            UpdateDelegateManager.AddUpdateDelegate(UpdateType.Update, StaticUpdate);
        }
        
        private static void StaticUpdate()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            {
                panelOnMouseClick = panelOnMouseHover;
            }
        }

        private void OnPanelCreated(IUIPanel panel)
        {
            if (panel is IUIPanelPointerEventProvider)
            {
                panel.OnOpenEvent += OnPanelOpen;
                panel.OnPostCloseEvent += OnPanelClose;
                panel.OnDestructEvent += OnPanelDestruct;
            }
        }

        private void OnPanelOpen(IUIPanel panel)
        {
            if (panel is IUIPanelPointerEventProvider pointerEventProvider)
            {
                pointerEventProvider.AddPointerEvent(OnPointerEnter, OnPointerLeave);
            }
        }

        private void OnPanelClose(IUIPanel panel)
        {
            if (panel is IUIPanelPointerEventProvider pointerEventProvider)
            {
                pointerEventProvider.RemovePointerEvent();
                
                OnPointerLeave(panel);
            }
        }

        private void OnPanelDestruct(IUIPanel panel)
        {
            if (panel is IUIPanelPointerEventProvider pointerEventProvider)
            {
                pointerEventProvider.RemovePointerEvent();
                
                OnPointerLeave(panel);
            }
        }

        private void OnPointerEnter(IUIPanel panel)
        {
            if (panel == null)
            {
                return;
            }
            
            panelOnMouseHover = panel;

            if (isDebugging)
            {
                Debug.LogWarning($"{name}鼠标进入");
            }

            if (panel is IUIPanelPointerEventReceiver receiver)
            {
                receiver.OnPointerEnter();
            }
        }

        private void OnPointerLeave(IUIPanel panel)
        {
            if (panel == null)
            {
                return;
            }
            
            if (panelOnMouseHover != panel)
            {
                return;
            }
            
            panelOnMouseHover = null;

            if (isDebugging)
            {
                Debug.LogWarning($"{name}鼠标离开");
            }

            if (panel is IUIPanelPointerEventReceiver receiver)
            {
                receiver.OnPointerLeave();
            }
        }
    }
}