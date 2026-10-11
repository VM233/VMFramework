using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine.Localization.Settings;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public class LocalizedUIPanelManager : ManagerBehaviour<LocalizedUIPanelManager>
    {
        [ShowInInspector]
        private readonly Dictionary<IUIPanel, LocalizedUIPanelSubscriptions> localizedModifiers = new();

        private UIPanelManager createdSource;
        private UIPanelManager destructedSource;
        private LocalizationSettings localization;

        protected override void Awake()
        {
            base.Awake();

            localizedModifiers.Clear();
        }

        protected override void OnDestroy()
        {
            var failures = new UIPanelReleaseFailures(nameof(LocalizedUIPanelManager));
            failures.Attempt("manager-subscriptions.release", ReleaseManagerSubscriptions);
            failures.Attempt("base.destroy", () => base.OnDestroy());
            failures.ThrowIfFailed();
        }

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            localization = LocalizationSettings.Instance;
            var panels = UIPanelManager.Instance;
            try
            {
                panels.OnPanelCreatedEvent += OnUIPanelCreated; createdSource = panels;
                panels.OnPanelDestructedEvent += OnUIPanelDestruct; destructedSource = panels;
            }
            catch (Exception primary)
            {
                var failures = new UIPanelReleaseFailures(nameof(LocalizedUIPanelManager), primary);
                failures.Attempt("partial-init.release", ReleaseManagerSubscriptions);
                failures.ThrowIfFailed(); throw;
            }
        }

        protected virtual void OnUIPanelCreated(IUIPanel uiPanelController)
        {
            var subscriptions = new LocalizedUIPanelSubscriptions(uiPanelController, localization);
            localizedModifiers.Add(uiPanelController, subscriptions);
            subscriptions.Acquire(OnUIPanelOpen, panel => subscriptions.ReleaseLocale());
        }

        protected virtual void OnUIPanelOpen(IUIPanel uiPanelController)
        {
            var subscriptions = localizedModifiers[uiPanelController];
            try
            {
            if (uiPanelController is ILocalizedPanelModifier localizedPanelController)
                subscriptions.Subscribe(localizedPanelController);
            foreach (var modifier in uiPanelController.Modifiers)
            {
                if (modifier is ILocalizedPanelModifier localizedPanelModifier)
                    subscriptions.Subscribe(localizedPanelModifier);
            }
            }
            catch (Exception primary)
            {
                var failures = new UIPanelReleaseFailures(nameof(LocalizedUIPanelManager), primary);
                failures.Attempt("partial-open.locale-release", subscriptions.ReleaseLocale);
                failures.ThrowIfFailed(); throw;
            }
        }

        protected virtual void OnUIPanelDestruct(IUIPanel uiPanelController)
        {
            // A failed Created fanout may not have reached this subscriber.
            if (!localizedModifiers.TryGetValue(uiPanelController, out var subscriptions)) return;
            localizedModifiers.Remove(uiPanelController);
            subscriptions.Release();
        }

        private void ReleaseManagerSubscriptions()
        {
            var created = createdSource; createdSource = null;
            var destructed = destructedSource; destructedSource = null;
            var actual = localizedModifiers.Values.ToArray(); localizedModifiers.Clear();
            localization = null;
            var failures = new UIPanelReleaseFailures(nameof(LocalizedUIPanelManager));
            if (!ReferenceEquals(created, null))
                failures.Attempt("created.unsubscribe", () => created.OnPanelCreatedEvent -= OnUIPanelCreated);
            if (!ReferenceEquals(destructed, null))
                failures.Attempt("destructed.unsubscribe", () => destructed.OnPanelDestructedEvent -= OnUIPanelDestruct);
            foreach (var subscriptions in actual)
                failures.Attempt("panel-subscriptions.release", subscriptions.Release);
            failures.ThrowIfFailed();
        }
    }
}
