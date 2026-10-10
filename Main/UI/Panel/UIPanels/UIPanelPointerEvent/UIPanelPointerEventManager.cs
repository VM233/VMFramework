using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Procedure;
using VMFramework.Timers;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public class UIPanelPointerEventManager : ManagerBehaviour<UIPanelPointerEventManager>
    {
        [SerializeField]
        private bool isDebugging;

        private UIPanelManager panels;
        private UpdateDelegateManager updates;
        private readonly HashSet<IUIPanel> watchedPanels = new();

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
            
            panels = UIPanelManager.Instance;
            panels.OnPanelCreatedEvent += OnPanelCreated;

            updates = UpdateDelegateManager.Instance;
            updates.OnUpdateEvent += StaticUpdate;
        }

        protected override void OnDestroy()
        {
            var panelSource = panels;
            var updateSource = updates;
            panels = null;
            updates = null;
            List<Exception> failures = null;
            try
            {
                if (!ReferenceEquals(panelSource, null))
                {
                    try { panelSource.OnPanelCreatedEvent -= OnPanelCreated; }
                    catch (Exception error) { (failures ??= new()).Add(error); }
                }
                if (!ReferenceEquals(updateSource, null))
                {
                    try { updateSource.OnUpdateEvent -= StaticUpdate; }
                    catch (Exception error) { (failures ??= new()).Add(error); }
                }
                foreach (var panel in watchedPanels)
                {
                    try { RetirePanel(panel); }
                    catch (Exception error) { (failures ??= new()).Add(error); }
                }
                if (failures != null) throw new AggregateException(failures);
            }
            finally
            {
                watchedPanels.Clear();
                base.OnDestroy();
            }
        }

        private static void StaticUpdate()
        {
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame ||
                Mouse.current.middleButton.wasPressedThisFrame)
            {
                panelOnMouseClick = panelOnMouseHover;
            }
#else
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            {
                panelOnMouseClick = panelOnMouseHover;
            }
#endif
        }

        private void OnPanelCreated(IUIPanel panel)
        {
            if (panel is IUIPanelPointerEventProvider)
            {
                watchedPanels.Add(panel);
                panel.OnOpen += OnPanelOpen;
                panel.OnPostClose += OnPanelClose;
                panel.OnDestruct += OnPanelDestruct;
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
            try { RetirePanel(panel); }
            finally { watchedPanels.Remove(panel); }
        }

        private void RetirePanel(IUIPanel panel)
        {
            panel.OnOpen -= OnPanelOpen;
            panel.OnPostClose -= OnPanelClose;
            panel.OnDestruct -= OnPanelDestruct;
            List<Exception> failures = null;
            try { ((IUIPanelPointerEventProvider)panel).RemovePointerEvent(); }
            catch (Exception error) { (failures ??= new()).Add(error); }
            try { OnPointerLeave(panel); }
            catch (Exception error) { (failures ??= new()).Add(error); }
            try
            {
                if (ReferenceEquals(panelOnMouseClick, panel)) panelOnMouseClick = null;
            }
            catch (Exception error) { (failures ??= new()).Add(error); }
            if (failures != null) throw new AggregateException(failures);
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
