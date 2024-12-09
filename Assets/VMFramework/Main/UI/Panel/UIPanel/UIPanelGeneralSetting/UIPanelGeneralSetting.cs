using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using VMFramework.Configuration;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    public sealed partial class UIPanelGeneralSetting : GamePrefabGeneralSetting, IInitializer
    {
        #region Category

        private const string PANEL_SETTING_CATEGORY = "Panel";

        #endregion

        #region MetaData

        public override Type BaseGamePrefabType => typeof(UIPanelConfig);

        #endregion

        public const string DEFAULT_SORTING_ORDER_ID = "Default";

        public const int DEFAULT_SORTING_ORDER = 0;
        
        public const string DEBUG_SORTING_ORDER_ID = "Debug";
        
        public const int DEBUG_SORTING_ORDER = 1000;

        [TabGroup(TAB_GROUP_NAME, PANEL_SETTING_CATEGORY)]
        public string containerName = "$UI";

        [TabGroup(TAB_GROUP_NAME, PANEL_SETTING_CATEGORY)]
        [Required]
        public ThemeStyleSheet defaultTheme;

        [TabGroup(TAB_GROUP_NAME, PANEL_SETTING_CATEGORY)]
        [JsonProperty]
        public PanelScreenMatchMode defaultScreenMatchMode = PanelScreenMatchMode.MatchWidthOrHeight;

        [TabGroup(TAB_GROUP_NAME, PANEL_SETTING_CATEGORY)]
        [PropertyRange(0, 1)]
        [JsonProperty]
        public float defaultMatch = 0.5f;

        [TabGroup(TAB_GROUP_NAME, PANEL_SETTING_CATEGORY)]
        [JsonProperty]
        public Vector2Int defaultReferenceResolution = new(1920, 1080);
        
        [TabGroup(TAB_GROUP_NAME, PANEL_SETTING_CATEGORY)]
        [JsonProperty]
        public DictionaryConfigs<string, PriorityPreset> sortingOrderPresets = new();

        [TabGroup(TAB_GROUP_NAME, PANEL_SETTING_CATEGORY)]
        [ShowInInspector]
        private Dictionary<int, PanelSettings> panelSettingsBySortingOrder = new();

        [TabGroup(TAB_GROUP_NAME, LOCALIZABLE_SETTING_CATEGORY)]
        [ToggleButtons("Enabled", "Disabled")]
        public bool enableLanguageConfigs = true;

        [TabGroup(TAB_GROUP_NAME, LOCALIZABLE_SETTING_CATEGORY)]
        [ShowIf(nameof(enableLanguageConfigs))]
        public DictionaryConfigs<string, UIPanelLanguageConfig> languageConfigs = new();

        #region Check & Init

        public override void CheckSettings()
        {
            base.CheckSettings();
            
            sortingOrderPresets.CheckSettings();

            if (enableLanguageConfigs)
            {
                languageConfigs.CheckSettings();
            }
        }
        
        protected override void OnInit()
        {
            base.OnInit();
            
            sortingOrderPresets.Init();
            
            languageConfigs.Init();

            panelSettingsBySortingOrder.Clear();

            foreach (var prefab in GamePrefabManager.GetAllGamePrefabs<IUIPanelConfig>())
            {
                if (panelSettingsBySortingOrder.ContainsKey(prefab.SortingOrder) == false)
                {
                    panelSettingsBySortingOrder[prefab.SortingOrder] = CreateInstance<PanelSettings>();
                }
            }

            foreach (var (sortingOrder, panelSetting) in panelSettingsBySortingOrder)
            {
                panelSetting.name = sortingOrder.ToString();
                panelSetting.sortingOrder = sortingOrder;
                panelSetting.themeStyleSheet = defaultTheme;
                panelSetting.scaleMode = PanelScaleMode.ScaleWithScreenSize;
                panelSetting.screenMatchMode = defaultScreenMatchMode;
                panelSetting.match = defaultMatch;
                panelSetting.referenceResolution = defaultReferenceResolution;
            }
        }

        #endregion
        
        private IEnumerable<PanelSettings> GetAllPanelSettings()
        {
            return panelSettingsBySortingOrder.Values;
        }

        public PanelSettings GetPanelSetting(int sortingOrder)
        {
            return panelSettingsBySortingOrder[sortingOrder];
        }
    }
}