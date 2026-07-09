#if UNITY_EDITOR
#pragma warning disable 0414
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Configuration;

namespace VMFramework.GameLogicArchitecture
{
    [OnInspectorInit("@((IInspectorConfig)$value)?.OnInspectorInit()")]
    public partial class GameSettingBase : IInspectorConfig
    {
        protected virtual void OnInspectorInit()
        {

        }

        void IInspectorConfig.OnInspectorInit()
        {
            OnInspectorInit();
        }

        private const string CHECK_SETTING_BUTTON_NAME = "Check Settings";

        protected virtual void CheckSettingsGUI()
        {
            CheckSettings();
        }

        private bool hasSettingError = false;

        private string checkSettingInfo = "Settings have not been checked yet!";

        protected virtual void OnValidate()
        {
            checkSettingInfo = "The settings have been updated, please click the check settings button again!";
        }

        [Title(CHECK_SETTING_BUTTON_NAME)]
        [InfoBox("@" + nameof(checkSettingInfo), InfoMessageType.Warning)]
        [HideInInlineEditors]
        [GUIColor(nameof(GetSettingCheckButtonColor)), PropertySpace(SpaceBefore = 10), PropertyOrder(999)]
        [Button(CHECK_SETTING_BUTTON_NAME, ButtonSizes.Large, Icon = SdfIconType.ArrowRightCircle)]
        private void CheckSettingsButton()
        {
            hasSettingError = true;
            checkSettingInfo = "The settings have been checked, please check the console for any errors!";

            CheckSettingsGUI();

            checkSettingInfo = "No errors found in the settings!";
            hasSettingError = false;
        }

        private Color GetSettingCheckButtonColor()
        {
            if (hasSettingError)
            {
                return new(1, 0, 0);
            }
            else
            {
                return new(0, 1, 0);
            }
        }
    }
}
#endif