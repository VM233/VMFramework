using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;
using VMFramework.Tools;
#if UNITY_EDITOR
using VMFramework.OdinExtensions;
#endif

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public class UIPanelManager : ManagerBehaviour<UIPanelManager>
    {
        public Transform UIContainer { get; private set; }

        public event Action<IUIPanel> OnPanelCreatedEvent;

        public event Action<IUIPanel> OnPanelDestructedEvent;

        [ShowInInspector]
        private readonly Dictionary<string, IUIPanel> uniquePanels = new();
        
        [ShowInInspector]
        private readonly Dictionary<string, HashSet<IUIPanel>> openedPanels = new();

        [ShowInInspector]
        private readonly HashSet<IUIPanel> closingPanels = new();

        private readonly List<IUIPanel> closedPanels = new();

        private readonly Dictionary<IUIPanel, Task> retiringRentals = new();

        #region Init

        protected override void Awake()
        {
            base.Awake();

            OnPanelCreatedEvent = null;
            OnPanelDestructedEvent = null;
            
            uniquePanels.Clear();
            openedPanels.Clear();
            closingPanels.Clear();
            closedPanels.Clear();
        }

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            UIContainer = ContainerTransform.Get(UISetting.UIPanelGeneralSetting.containerName);
        }

        #endregion

        #region Register & Unregister

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Register(IUIPanel panel)
        {
            panel.AdoptRegistration(this);
            try
            {
                if (panel.IsUnique) uniquePanels.Add(panel.id, panel);
                OnPanelCreatedEvent?.Invoke(panel);
            }
            catch (Exception primary)
            {
                var failures = new UIPanelReleaseFailures(nameof(UIPanelManager), primary);
                if (ReferenceEquals(panel.RegistrationOwner, this))
                    failures.Attempt("unadopted-registration.unregister", () => Unregister(panel));
                failures.ThrowIfFailed(); throw;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Unregister(IUIPanel panel)
        {
            panel.WithdrawRegistration(this);
            if (panel.IsUnique && uniquePanels.TryGetValue(panel.id, out var actual) && ReferenceEquals(actual, panel))
            {
                uniquePanels.Remove(panel.id);
            }
            var failures = new UIPanelReleaseFailures(nameof(UIPanelManager));
            var subscribers = OnPanelDestructedEvent;
            if (subscribers != null)
                foreach (Action<IUIPanel> subscriber in subscribers.GetInvocationList())
                    try { subscriber(panel); }
                    catch (Exception error) { failures.Capture("panel-destructed.subscriber", error); }
            failures.ThrowIfFailed();
        }

        #endregion

        #region Open & Close

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Toggle(IUIPanel panel)
        {
            if (panel.IsOpened)
            {
                Close(panel);
            }
            else
            {
                Open(panel, null);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Open(IUIPanel panel, IUIPanel source)
        {
            TryOpen(panel, source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryOpen(IUIPanel panel, IUIPanel source)
        {
            if (retiringRentals.ContainsKey(panel))
                throw new InvalidOperationException($"Panel {panel.id} has a rental already handed to retirement.");
            if (!ReferenceEquals(panel.RegistrationOwner, this))
                throw new InvalidOperationException($"Panel {panel.id} Open requires this manager's completed rental admission.");
            try
            {
            if (closingPanels.Remove(panel))
            {
                panel.OnPostCloseInternal();
            }
            panel.OnOpenInternal(source);
            var set = openedPanels.GetOrCreateFromFactory(panel.id, HashSetPoolFactory<IUIPanel>.CreateFromDefaultPool);
            set.Add(panel);
            return true;
            }
            catch (Exception primary)
            {
                var failures = new UIPanelReleaseFailures(nameof(UIPanelManager), primary);
                failures.Attempt("open-prefix.retire", () => RetireFailedOpen(panel, primary));
                failures.ThrowIfFailed(); throw;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryClose(IUIPanel panel)
        {
            if (!panel.IsOpened) return false;
            if (!closingPanels.Add(panel)) return false;
            try { panel.OnPreCloseInternal(); }
            catch (Exception failure) { RetireFailedOpen(panel, failure); throw; }
            RemoveOpened(panel);
            return true;
        }

        private void RemoveOpened(IUIPanel panel)
        {
            if (openedPanels.TryGetValue(panel.id, out var set)) set.Remove(panel);
        }

        internal void RetireFailedOpen(IUIPanel panel, Exception failure)
        {
            bool closingOwned = closingPanels.Remove(panel);
            bool ownsRental = ReferenceEquals(panel.RegistrationOwner, this);
            if (openedPanels.TryGetValue(panel.id, out var set)) set.Remove(panel);
            var failures = new UIPanelReleaseFailures(nameof(UIPanelManager), failure);
            failures.Attempt("failed-open.retire", () =>
            {
                if (closingOwned) panel.RetireFailedClose(failure);
                else panel.RetireFailedOpen(failure);
            });
            if (ownsRental && !panel.IsUnique)
                failures.Attempt("failed-open.return", () => ReturnAfterLayout(panel, failure));
            failures.ThrowIfReleaseFailed();
        }

        internal void RetireDestroyedPanel(IUIPanel panel)
        {
            bool closingOwned = closingPanels.Remove(panel);
            bool openedOwned = openedPanels.TryGetValue(panel.id, out var set) && set.Remove(panel);
            // Native destruction consumes custody of this rental. A previously
            // registered Task continuation must no longer return it to the pool.
            retiringRentals.Remove(panel);
            if (closingOwned || openedOwned || panel.IsOpened || panel.IsClosing)
                panel.PublishDestroyedClose(closingOwned);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Close(IUIPanel panel) => TryClose(panel);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsClosing(IUIPanel panel) => closingPanels.Contains(panel);

        private void OnPanelClosing(IUIPanel panel)
        {
            var failures = new UIPanelReleaseFailures(nameof(UIPanelManager));
            failures.Attempt("post-close.publish", panel.OnPostCloseInternal);
            if (!panel.IsUnique) failures.Attempt("closed-rental.return", () => ReturnAfterLayout(panel, failures.PrimaryFailure));
            failures.ThrowIfFailed();
        }

        private void ReturnAfterLayout(IUIPanel panel, Exception sourceCause)
        {
            // Controller lifetime retirement has already consumed this rental.
            if (panel.IsDestroyed) return;
            var toolkit = panel as IUIToolkitPanel;
            var actualLayout = toolkit?.LayoutCompletion;
            var token = toolkit?.LayoutCancellationToken ?? default;
            var provider = GameItemManager.Instance;
            if (actualLayout == null || actualLayout.IsCompleted)
            {
                CompleteRentalRetirement(panel, toolkit, actualLayout, token, provider, sourceCause);
                return;
            }
            retiringRentals.Add(panel, actualLayout);
            var awaiter = actualLayout.GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                if (!retiringRentals.Remove(panel)) return;
                try { CompleteRentalRetirement(panel, toolkit, actualLayout, token, provider, sourceCause); }
                catch (Exception failure) { Debug.LogException(failure, this); }
            });
        }

        private static void CompleteRentalRetirement(IUIPanel panel, IUIToolkitPanel toolkit,
            Task actualLayout, CancellationToken token, IGameItemManager provider, Exception sourceCause)
        {
            Exception layoutFailure = null, rootFailure = null, returnFailure = null;
            if (actualLayout != null)
            {
                try { actualLayout.GetAwaiter().GetResult(); }
                catch (OperationCanceledException cancellation) when
                    (cancellation.CancellationToken == token && token.IsCancellationRequested) { }
                catch (Exception failure) { layoutFailure = failure; }
            }
            try { toolkit?.CompleteLayoutRetirement(actualLayout); }
            catch (Exception failure) { rootFailure = failure; }
            try { if (!panel.IsDestroyed) provider.Return(panel); }
            catch (Exception failure) { returnFailure = failure; }
            var primary = sourceCause ?? layoutFailure ?? rootFailure ?? returnFailure;
            if (primary == null) return;
            if (layoutFailure != null || rootFailure != null || returnFailure != null)
                primary.Data[typeof(UIPanelRentalRetirementFailure)] = new UIPanelRentalRetirementFailure(
                    actualLayout, sourceCause, layoutFailure, rootFailure, returnFailure);
            // The source caller already propagates its cause. This consumer
            // reports only a newly observed source result or actual release failure.
            if (sourceCause == null || rootFailure != null || returnFailure != null ||
                (layoutFailure != null && !ReferenceEquals(layoutFailure, sourceCause)))
                ExceptionDispatchInfo.Capture(primary).Throw();
        }

        private void Update()
        {
            foreach (var panel in closingPanels)
                if (!panel.IsOpened) closedPanels.Add(panel);
            var failures = new UIPanelReleaseFailures(nameof(UIPanelManager));
            foreach (var panel in closedPanels)
            {
                if (closingPanels.Remove(panel))
                    failures.Attempt("closed-panel.retire", () => OnPanelClosing(panel));
            }
            closedPanels.Clear();
            failures.ThrowIfFailed();
        }

        #endregion

        #region Opened Panels

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetOpenedPanels(string id, out IReadOnlyCollection<IUIPanel> panels)
        {
            if (openedPanels.TryGetValue(id, out var set))
            {
                if (set.Count > 0)
                {
                    panels = set;
                    return true;
                }
            }
            
            panels = null;
            return false;
        }

        #endregion

        #region Unique Panel

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetUniquePanels<TPanel>(ICollection<TPanel> panels)
        {
            foreach (var panel in uniquePanels.Values)
            {
                if (panel is TPanel typedPanel)
                {
                    panels.Add(typedPanel);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TPanel GetUniquePanel<TPanel>(string id)
            where TPanel : IUIPanel
        {
            if (uniquePanels.TryGetValue(id, out var panel) == false)
            {
                return default;
            }

            if (panel is TPanel typedPanel)
            {
                return typedPanel;
            }

            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IUIPanel GetUniquePanelStrictly(string id)
        {
            var panelController = uniquePanels.GetValueOrDefault(id);
            panelController.AssertIsNotNull(nameof(panelController));
            return panelController;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TPanel GetUniquePanelStrictly<TPanel>(string id)
            where TPanel : IUIPanel
        {
            var panel = GetUniquePanel<TPanel>(id);

            if (panel == null)
            {
                throw new Exception($"The unique panel with ID {id} does not exist");
            }

            return panel;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetUniquePanel(string id, out IUIPanel panel)
        {
            return uniquePanels.TryGetValue(id, out panel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetUniquePanel<TPanel>(string id, out TPanel panel)
            where TPanel : IUIPanel
        {
            if (uniquePanels.TryGetValue(id, out var panelInterface))
            {
                if (panelInterface is TPanel typedPanel)
                {
                    panel = typedPanel;
                    return true;
                }
            }

            panel = default;
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetUniquePanelWithWarning(string id, out IUIPanel panel)
        {
            if (uniquePanels.TryGetValue(id, out panel) == false)
            {
                UnityEngine.Debug.LogWarning($"The unique panel with ID {id} does not exist");
                return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetUniquePanelWithWarning<TPanel>(string id, out TPanel panel)
            where TPanel : IUIPanel
        {
            if (uniquePanels.TryGetValue(id, out var panelInterface) == false)
            {
                UnityEngine.Debug.LogWarning($"The unique panel with ID {id} does not exist");
                panel = default;
                return false;
            }

            if (panelInterface is not TPanel typedPanel)
            {
                UnityEngine.Debug.LogWarning($"The unique panel with ID {id} is {panelInterface.GetType()} " +
                                    $"instead of {typeof(TPanel)}");
                panel = default;
                return false;
            }

            panel = typedPanel;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IUIPanel GetAndOpenUniquePanel(string id)
        {
            if (uniquePanels.TryGetValue(id, out var panel))
            {
                TryOpen(panel, null);
                return panel;
            }

            return null;
        }

        #endregion

        #region Get And Open

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IUIPanel GetAndOpen(string id, IUIPanel source = null)
        {
            if (uniquePanels.TryGetValue(id, out var panel))
            {
                TryOpen(panel, source);
                return panel;
            }
            
            return CreateAndOpen(id, source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TPanel GetAndOpen<TPanel>(string id) 
            where TPanel : IUIPanel
        {
            if (uniquePanels.TryGetValue(id, out var panel))
            {
                if (panel is not TPanel typedPanel)
                {
                    throw new ArgumentException($"The unique panel with ID {id} is not of type {typeof(TPanel)}");
                }
                
                TryOpen(panel, null);
                return typedPanel;
            }
            
            return CreateAndOpen<TPanel>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IUIPanel CreateAndOpen(string id, IUIPanel source = null) => CreateAndOpen<IUIPanel>(id, source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TPanel CreateAndOpen<TPanel>(string id, IUIPanel source = null) where TPanel : IUIPanel
        {
            var panel = CreatePanel<TPanel>(id);
            TryOpen(panel, source);
            return panel;
        }

        public IUIPanel CreatePanel(string id) => CreatePanel<IUIPanel>(id);

        public TPanel CreatePanel<TPanel>(string id) where TPanel : IUIPanel
        {
            var provider = GameItemManager.Instance;
            var panel = provider.Get<TPanel>(id);
            AdoptRental(panel, provider);
            return panel;
        }

        private void AdoptRental(IUIPanel panel, IGameItemManager provider)
        {
            try { Register(panel); }
            catch (Exception primary)
            {
                var failures = new UIPanelReleaseFailures(nameof(UIPanelManager), primary);
                if (!panel.IsDestroyed)
                    failures.Attempt("unadopted-registration-rental.return", () => provider.Return(panel));
                failures.ThrowIfFailed(); throw;
            }
        }

        #endregion

#if UNITY_EDITOR
        [Button]
        private void _CreateAndOpen([GamePrefabID(typeof(IUIPanelConfig))] string id) => CreateAndOpen(id);
#endif

        #region Reenter

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Reenter(string id)
        {
            if (uniquePanels.TryGetValue(id, out var panel))
            {
                panel.Reenter();
            }
        }

        #endregion
    }
}
