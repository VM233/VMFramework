using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Localization.Settings;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public class LocalizedUIPanelManager : ManagerBehaviour<LocalizedUIPanelManager>
    {
        [ShowInInspector]
        protected readonly Dictionary<IUIPanel, List<ILocalizedPanelModifier>> localizedModifiers = new();

        private UIPanelManager panels;
        private LocalizationSettings localization;

        protected override void Awake()
        {
            base.Awake();

            localizedModifiers.Clear();
        }

        protected override void OnDestroy()
        {
            var source = panels;
            panels = null;
            List<Exception> failures = null;
            try
            {
                if (!ReferenceEquals(source, null))
                {
                    try { source.OnPanelCreatedEvent -= OnUIPanelCreated; }
                    catch (Exception error) { (failures ??= new()).Add(error); }
                }
                foreach (var pair in localizedModifiers)
                {
                    try { RetirePanel(pair.Key, pair.Value); }
                    catch (Exception error) { (failures ??= new()).Add(error); }
                }
                if (failures != null) throw new AggregateException(failures);
            }
            finally
            {
                localizedModifiers.Clear();
                localization = null;
                base.OnDestroy();
            }
        }

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            panels = UIPanelManager.Instance;
            localization = LocalizationSettings.Instance;
            panels.OnPanelCreatedEvent += OnUIPanelCreated;
        }

        protected virtual void OnUIPanelCreated(IUIPanel uiPanelController)
        {
            localizedModifiers.Add(uiPanelController, new());
            uiPanelController.OnOpen += OnUIPanelOpen;
            uiPanelController.OnPostClose += OnUIPanelClose;
            uiPanelController.OnDestruct += OnUIPanelDestruct;
        }

        protected virtual void OnUIPanelOpen(IUIPanel uiPanelController)
        {
            var subscriptions = localizedModifiers[uiPanelController];
            if (uiPanelController is ILocalizedPanelModifier localizedPanelController)
                Subscribe(localizedPanelController, subscriptions);
            foreach (var modifier in uiPanelController.Modifiers)
            {
                if (modifier is ILocalizedPanelModifier localizedPanelModifier)
                    Subscribe(localizedPanelModifier, subscriptions);
            }
        }

        private void Subscribe(ILocalizedPanelModifier modifier, List<ILocalizedPanelModifier> subscriptions)
        {
            modifier.OnCurrentLanguageChanged(localization.GetSelectedLocale());
            localization.OnSelectedLocaleChanged += modifier.OnCurrentLanguageChanged;
            subscriptions.Add(modifier);
        }

        protected virtual void OnUIPanelClose(IUIPanel uiPanelController)
        {
            var subscriptions = localizedModifiers[uiPanelController];
            foreach (var modifier in subscriptions)
                localization.OnSelectedLocaleChanged -= modifier.OnCurrentLanguageChanged;
            subscriptions.Clear();
        }

        protected virtual void OnUIPanelDestruct(IUIPanel uiPanelController)
        {
            var subscriptions = localizedModifiers[uiPanelController];
            RetirePanel(uiPanelController, subscriptions);
            localizedModifiers.Remove(uiPanelController);
        }

        private void RetirePanel(IUIPanel panel, List<ILocalizedPanelModifier> subscriptions)
        {
            panel.OnOpen -= OnUIPanelOpen;
            panel.OnPostClose -= OnUIPanelClose;
            panel.OnDestruct -= OnUIPanelDestruct;
            foreach (var modifier in subscriptions)
                localization.OnSelectedLocaleChanged -= modifier.OnCurrentLanguageChanged;
            subscriptions.Clear();
        }
    }
}
