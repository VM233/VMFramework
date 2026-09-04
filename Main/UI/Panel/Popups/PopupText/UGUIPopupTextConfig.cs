using System;

namespace VMFramework.UI
{
    [Serializable]
    public class UGUIPopupTextConfig : UGUIPanelConfig, IPopupTextConfig
    {
        public override Type GameItemType => typeof(UGUIPopupText);

        
    }
}
