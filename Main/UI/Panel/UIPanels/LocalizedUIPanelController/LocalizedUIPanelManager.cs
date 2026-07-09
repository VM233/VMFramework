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
        protected readonly HashSet<ILocalizedPanelModifier> localizedModifiers = new();

        protected override void Awake()
        {
            base.Awake();

            localizedModifiers.Clear();
        }

        protected virtual void OnDestroy()
        {
            foreach (var localizedPanelModifier in localizedModifiers)
            {
                LocalizationSettings.SelectedLocaleChanged -= localizedPanelModifier.OnCurrentLanguageChanged;
            }
        }

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            UIPanelManager.Instance.OnPanelCreatedEvent += OnUIPanelCreated;
        }

        protected virtual void OnUIPanelCreated(IUIPanel uiPanelController)
        {
            uiPanelController.OnOpen += OnUIPanelOpen;
            uiPanelController.OnPostClose += OnUIPanelClose;
            uiPanelController.OnDestruct += OnUIPanelDestruct;
        }

        protected virtual void OnUIPanelOpen(IUIPanel uiPanelController)
        {
            foreach (var modifier in uiPanelController.Modifiers)
            {
                if (modifier is ILocalizedPanelModifier localizedPanelModifier)
                {
                    localizedPanelModifier.OnCurrentLanguageChanged(LocalizationSettings.SelectedLocale);

                    LocalizationSettings.SelectedLocaleChanged += localizedPanelModifier.OnCurrentLanguageChanged;

                    localizedModifiers.Add(localizedPanelModifier);
                }
            }
        }

        protected virtual void OnUIPanelClose(IUIPanel uiPanelController)
        {
            foreach (var modifier in uiPanelController.Modifiers)
            {
                if (modifier is ILocalizedPanelModifier localizedPanelModifier)
                {
                    LocalizationSettings.SelectedLocaleChanged -= localizedPanelModifier.OnCurrentLanguageChanged;

                    localizedModifiers.Remove(localizedPanelModifier);
                }
            }
        }

        protected virtual void OnUIPanelDestruct(IUIPanel uiPanelController)
        {
            foreach (var modifier in uiPanelController.Modifiers)
            {
                if (modifier is ILocalizedPanelModifier localizedPanelModifier)
                {
                    LocalizationSettings.SelectedLocaleChanged -= localizedPanelModifier.OnCurrentLanguageChanged;

                    localizedModifiers.Remove(localizedPanelModifier);
                }
            }
        }
    }
}