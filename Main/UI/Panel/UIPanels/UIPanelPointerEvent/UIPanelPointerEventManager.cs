using System;
using System.Collections.Generic;
using System.Linq;
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

        private UIPanelManager createdSource;
        private UIPanelManager destructedSource;
        private UpdateDelegateManager updates;
        private readonly Dictionary<IUIPanel, UIPanelPointerWatch> watchedPanels = new();

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
            
            var panels = UIPanelManager.Instance;
            var updateSource = UpdateDelegateManager.Instance;
            try
            {
                panels.OnPanelCreatedEvent += OnPanelCreated; createdSource = panels;
                panels.OnPanelDestructedEvent += OnPanelDestruct; destructedSource = panels;
                updateSource.OnUpdateEvent += StaticUpdate; updates = updateSource;
            }
            catch (Exception primary)
            {
                var failures = new UIPanelReleaseFailures(nameof(UIPanelPointerEventManager), primary);
                failures.Attempt("partial-init.release", ReleaseManagerSubscriptions);
                failures.ThrowIfFailed(); throw;
            }
        }

        protected override void OnDestroy()
        {
            var failures = new UIPanelReleaseFailures(nameof(UIPanelPointerEventManager));
            failures.Attempt("manager-subscriptions.release", ReleaseManagerSubscriptions);
            failures.Attempt("base.destroy", () => base.OnDestroy());
            failures.ThrowIfFailed();
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
            if (panel is IUIPanelPointerEventProvider provider)
            {
                var watch = new UIPanelPointerWatch(panel, provider);
                watchedPanels.Add(panel, watch);
                watch.Acquire(OnPanelOpen, closed => OnPanelClose(closed, watch));
            }
        }

        private void OnPanelOpen(IUIPanel panel)
        {
            watchedPanels[panel].Activate(OnPointerEnter, OnPointerLeave);
        }

        private void OnPanelClose(IUIPanel panel, UIPanelPointerWatch watch)
        {
            var failures = new UIPanelReleaseFailures(nameof(UIPanelPointerEventManager));
            failures.Attempt("pointer.release", watch.ReleasePointer);
            failures.Attempt("pointer.leave", () => OnPointerLeave(panel));
            failures.ThrowIfFailed();
        }

        private void OnPanelDestruct(IUIPanel panel)
        {
            if (!watchedPanels.TryGetValue(panel, out var watch)) return;
            watchedPanels.Remove(panel);
            RetirePanel(panel, watch);
        }

        private void RetirePanel(IUIPanel panel, UIPanelPointerWatch watch)
        {
            var failures = new UIPanelReleaseFailures(nameof(UIPanelPointerEventManager));
            failures.Attempt("watch.release", watch.Release);
            failures.Attempt("pointer.leave", () => OnPointerLeave(panel));
            if (ReferenceEquals(panelOnMouseClick, panel))
                failures.Attempt("click.clear", () => panelOnMouseClick = null);
            failures.ThrowIfFailed();
        }

        private void ReleaseManagerSubscriptions()
        {
            var created = createdSource; createdSource = null;
            var destructed = destructedSource; destructedSource = null;
            var updated = updates; updates = null;
            var actual = watchedPanels.ToArray(); watchedPanels.Clear();
            var failures = new UIPanelReleaseFailures(nameof(UIPanelPointerEventManager));
            if (!ReferenceEquals(created, null))
                failures.Attempt("created.unsubscribe", () => created.OnPanelCreatedEvent -= OnPanelCreated);
            if (!ReferenceEquals(destructed, null))
                failures.Attempt("destructed.unsubscribe", () => destructed.OnPanelDestructedEvent -= OnPanelDestruct);
            if (!ReferenceEquals(updated, null))
                failures.Attempt("update.unsubscribe", () => updated.OnUpdateEvent -= StaticUpdate);
            foreach (var pair in actual)
                failures.Attempt("panel-watch.release", () => RetirePanel(pair.Key, pair.Value));
            failures.ThrowIfFailed();
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
