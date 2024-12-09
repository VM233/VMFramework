using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.GameEvents;

namespace VMFramework.UI
{
    public sealed class ContainerToggleOnEventTriggeredPanelModifier : PanelModifier
    {
        private ContainerToggleOnEventTriggeredPanelModifierConfig ModifierConfig =>
            (ContainerToggleOnEventTriggeredPanelModifierConfig)GamePrefab;

        [ShowInInspector]
        private VisualElement container;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            
            Panel.OnOpenEvent += OnOpen;
            Panel.OnPostCloseEvent += OnClose;
        }

        private void OnOpen(IUIPanel panel)
        {
            var uiToolkitPanel = (IUIToolkitPanel)panel;

            container = uiToolkitPanel.RootVisualElement.QueryStrictly(ModifierConfig.containerName,
                nameof(uiToolkitPanel.RootVisualElement));
            
            GameEventManager.AddCallback<bool>(ModifierConfig.containerToggleGameEventID, OnContainerToggle, GameEventPriority.TINY);
        }

        private void OnClose(IUIPanel panel)
        {
            GameEventManager.RemoveCallback<bool>(ModifierConfig.containerToggleGameEventID, OnContainerToggle);
        }

        private void OnContainerToggle(bool value)
        {
            if (value == false)
            {
                return;
            }
                
            container.ToggleDisplay();
        }
    }
}