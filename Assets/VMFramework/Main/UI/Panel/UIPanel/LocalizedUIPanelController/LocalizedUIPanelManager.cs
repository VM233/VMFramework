using UnityEngine.Localization.Settings;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public sealed class LocalizedUIPanelManager : ManagerBehaviour<LocalizedUIPanelManager>
    {
        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();
            
            UIPanelManager.OnPanelCreatedEvent += OnUIPanelCreated;
        }

        private void OnUIPanelCreated(IUIPanel uiPanelController)
        {
            if (uiPanelController is ILocalizedUIPanel)
            {
                uiPanelController.OnOpenEvent += OnUIPanelOpen;
                uiPanelController.OnPostCloseEvent += OnUIPanelClose;
                uiPanelController.OnDestructEvent += OnUIPanelDestruct;
            }
        }

        private void OnUIPanelOpen(IUIPanel uiPanelController)
        {
            if (uiPanelController is ILocalizedUIPanel localizedUIPanelController)
            {
                localizedUIPanelController.OnCurrentLanguageChanged(LocalizationSettings.SelectedLocale);

                LocalizationSettings.SelectedLocaleChanged +=
                    localizedUIPanelController.OnCurrentLanguageChanged;
            }
        }

        private void OnUIPanelClose(IUIPanel uiPanelController)
        {
            if (uiPanelController is ILocalizedUIPanel localizedUIPanelController)
            {
                LocalizationSettings.SelectedLocaleChanged -=
                    localizedUIPanelController.OnCurrentLanguageChanged;
            }
        }

        private void OnUIPanelDestruct(IUIPanel uiPanelController)
        {
            if (uiPanelController is ILocalizedUIPanel localizedUIPanelController)
            {
                LocalizationSettings.SelectedLocaleChanged -=
                    localizedUIPanelController.OnCurrentLanguageChanged;
            }
        }
    }
}