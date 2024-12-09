using System;
using VMFramework.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.UI
{
    public class UGUIPanelConfig : UIPanelConfig, IUGUIPanelConfig
    {
        protected const string UGUI_PANEL_CATEGORY = "UGUI";

        public override Type GameItemType => typeof(UGUIPanel);

        [TabGroup(TAB_GROUP_NAME, UGUI_PANEL_CATEGORY)]
        [AssetsOnly]
        [Required]
        public GameObject uguiAsset;

        public GameObject UGUIAsset => uguiAsset;

        public override void CheckSettings()
        {
            base.CheckSettings();

            uguiAsset.AssertIsNotNull(nameof(uguiAsset));
        }
    }
}
