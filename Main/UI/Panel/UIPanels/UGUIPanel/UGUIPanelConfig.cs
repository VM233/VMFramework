using System;

namespace VMFramework.UI
{
    public class UGUIPanelConfig : UIPanelConfig, IUGUIPanelConfig
    {
        protected const string UGUI_PANEL_CATEGORY = "UGUI";

        public override Type GameItemType => typeof(UGUIPanel);
    }
}
