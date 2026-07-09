using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.Core;

namespace VMFramework.UI
{
    public class UIToolkitPanelConfig : UIPanelConfig, IUIToolkitPanelConfig
    {
        public const string UI_TOOLKIT_CATEGORY = "UI Toolkit";

        public override Type GameItemType => typeof(UIToolkitPanel);

        public PanelSettings PanelSettings
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (useDefaultPanelSettings)
                {
                    return UISetting.UIPanelGeneralSetting.GetPanelSetting(sortingOrder);
                }

                return customPanelSettings;
            }
        }

        [TabGroup(TAB_GROUP_NAME, UI_TOOLKIT_CATEGORY, SdfIconType.ColumnsGap, TextColor = "red")]
        [JsonProperty]
        public bool useDefaultPanelSettings = true;

        [TabGroup(TAB_GROUP_NAME, UI_TOOLKIT_CATEGORY)]
        [HideIf(nameof(useDefaultPanelSettings))]
        [Required]
        public PanelSettings customPanelSettings;

        [TabGroup(TAB_GROUP_NAME, UI_TOOLKIT_CATEGORY)]
        [JsonProperty]
        public bool ignoreMouseEvents;

        [TabGroup(TAB_GROUP_NAME, UI_TOOLKIT_CATEGORY)]
        [JsonProperty]
        public UIToolkitPanelCloseMode closeMode;

        public override void CheckSettings()
        {
            base.CheckSettings();
            
            if (useDefaultPanelSettings == false)
            {
                customPanelSettings.AssertIsNotNull(nameof(customPanelSettings));
            }
        }

        #region Interface Implementation

        VisualTreeAsset IVisualTreeAssetProvider.VisualTree
        {
            get
            {
                if (prefab == null)
                {
                    return null;
                }

                if (prefab.TryGetComponent(out UIDocument uiDocument) == false)
                {
                    return null;
                }
                
                return uiDocument.visualTreeAsset;
            }
        }

        bool IUIToolkitPanelConfig.IgnoreMouseEvents => ignoreMouseEvents;
        
        UIToolkitPanelCloseMode IUIToolkitPanelConfig.CloseMode => closeMode;

        #endregion
    }
}