using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;

namespace VMFramework.UI
{
    public sealed class UICloseButtonPanelModifier : UIToolkitPanelModifier
    {
        private UICloseButtonPanelModifierConfig ModifierConfig => (UICloseButtonPanelModifierConfig)GamePrefab;

        [ShowInInspector]
        private Button closeButton;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            Panel.OnOpenEvent += OnOpen;
            Panel.OnPostCloseEvent += OnClose;
        }

        private void OnOpen(IUIPanel panel)
        {
            closeButton = RootVisualElement.QueryStrictly<Button>(ModifierConfig.closeUIButtonName,
                nameof(ModifierConfig.closeUIButtonName));

            closeButton.clicked += panel.Close;
        }

        private void OnClose(IUIPanel panel)
        {
            if (closeButton != null)
            {
                closeButton.clicked -= panel.Close;

                closeButton = null;
            }
        }
    }
}