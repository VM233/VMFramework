using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public class UIToolkitPanelConfig : UIPanelConfig, IUIToolkitPanelConfig
    {
        public const string UI_TOOLKIT_PANEL_CATEGORY = "UI Toolkit";

        public override Type GameItemType => typeof(UIToolkitPanel);

        public PanelSettings PanelSettings
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (useDefaultPanelSettings)
                {
                    return UISetting.UIPanelGeneralSetting.GetPanelSetting(sortingOrderConfig.GetPriority());
                }

                return customPanelSettings;
            }
        }

        [SuffixLabel("UXML File")]
        [TabGroup(TAB_GROUP_NAME, UI_TOOLKIT_PANEL_CATEGORY, SdfIconType.ColumnsGap, TextColor = "red")]
        [Required]
        public VisualTreeAsset visualTree;

        [TabGroup(TAB_GROUP_NAME, UI_TOOLKIT_PANEL_CATEGORY)]
        [JsonProperty]
        public bool useDefaultPanelSettings = true;

        [TabGroup(TAB_GROUP_NAME, UI_TOOLKIT_PANEL_CATEGORY)]
        [HideIf(nameof(useDefaultPanelSettings))]
        [Required]
        public PanelSettings customPanelSettings;

        [TabGroup(TAB_GROUP_NAME, UI_TOOLKIT_PANEL_CATEGORY)]
        [JsonProperty]
        public bool ignoreMouseEvents;

        [TabGroup(TAB_GROUP_NAME, UI_TOOLKIT_PANEL_CATEGORY)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        public string uiMainPartName;

        public override void CheckSettings()
        {
            base.CheckSettings();

            visualTree.AssertIsNotNull(nameof(visualTree));

            if (useDefaultPanelSettings == false)
            {
                customPanelSettings.AssertIsNotNull(nameof(customPanelSettings));
            }
        }

        #region Interface Implementation

        VisualTreeAsset IVisualTreeAssetProvider.VisualTree => visualTree;

        bool IUIToolkitPanelConfig.IgnoreMouseEvents => ignoreMouseEvents;

        string IUIToolkitPanelConfig.UIMainPartName => uiMainPartName;

        #endregion
    }
}