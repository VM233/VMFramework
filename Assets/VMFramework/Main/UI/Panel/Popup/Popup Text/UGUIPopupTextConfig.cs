using System;
using Sirenix.OdinInspector;
using TMPro;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class UGUIPopupTextConfig : UGUIPopupConfig, IPopupTextConfig
    {
        public override Type GameItemType => typeof(UGUIPopupText);

        [TabGroup(TAB_GROUP_NAME, POPUP_SETTING_CATEGORY)]
        [UGUIName(typeof(TextMeshProUGUI))]
        public string textName;
    }
}
