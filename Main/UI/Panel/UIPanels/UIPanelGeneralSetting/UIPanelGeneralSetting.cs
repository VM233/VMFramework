using System;
using System.Collections.Generic;
using VMFramework.Configuration;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;
using VMFramework.Procedure;
#if UNITY_EDITOR
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
#endif

namespace VMFramework.UI
{
    [CommonPresetAutoRegister(SORTING_ORDER_PRESET_KEY, typeof(int))]
    public sealed class UIPanelGeneralSetting : GamePrefabGeneralSetting, IInitializer
#if UNITY_EDITOR
        , IGameEditorMenuTreeNode
#endif
    {
        #region Category

        private const string PANEL_SETTING_CATEGORY = "Panel";

        #endregion

        #region MetaData

        public override Type BaseGamePrefabType => typeof(UIPanelConfig);

        #endregion

        public const string SORTING_ORDER_PRESET_KEY = "UI Sorting Order";

        public const string DEFAULT_SORTING_ORDER_ID = "Default";

        public const int DEFAULT_SORTING_ORDER = 0;

        public const string DEBUG_SORTING_ORDER_ID = "Debug";

        public const int DEBUG_SORTING_ORDER = 1000;

        [TabGroup(TAB_GROUP_NAME, PANEL_SETTING_CATEGORY)]
        public string containerName = "$UI";

        [TabGroup(TAB_GROUP_NAME, PANEL_SETTING_CATEGORY)]
        [Required]
        public PanelSettings panelSettings;

        [TabGroup(TAB_GROUP_NAME, PANEL_SETTING_CATEGORY)]
        [ShowInInspector, HideInEditorMode]
        private Dictionary<int, PanelSettings> panelSettingsBySortingOrder = new();

        [TabGroup(TAB_GROUP_NAME, LOCALIZABLE_SETTING_CATEGORY)]
        [ToggleButtons("Enabled", "Disabled")]
        public bool enableLanguageConfigs = true;

        [TabGroup(TAB_GROUP_NAME, LOCALIZABLE_SETTING_CATEGORY)]
        [ShowIf(nameof(enableLanguageConfigs))]
        [SerializeReference]
        public List<UIPanelLanguageConfig> languageConfigs = new();

        #region Check & Init

        public override void CheckSettings()
        {
            base.CheckSettings();

            if (enableLanguageConfigs)
            {
                languageConfigs.CheckUniqueIDs(nameof(languageConfigs));
                languageConfigs.CheckSettings();
            }
        }

        protected override void OnInit()
        {
            base.OnInit();

            languageConfigs.CheckUniqueIDs(nameof(languageConfigs));
            languageConfigs.Init();

            panelSettingsBySortingOrder.Clear();

            if (panelSettings == null)
            {
                panelSettings = CreateInstance<PanelSettings>();
                panelSettings.scaleMode = PanelScaleMode.ScaleWithScreenSize;
                panelSettings.screenMatchMode = PanelScreenMatchMode.MatchWidthOrHeight;
                panelSettings.match = 0.5f;
                panelSettings.referenceResolution = new Vector2Int(1920, 1080);
            }

            foreach (var prefab in GamePrefabManager.GetAllGamePrefabs<IUIPanelConfig>())
            {
                if (panelSettingsBySortingOrder.ContainsKey(prefab.SortingOrder) == false)
                {
                    panelSettingsBySortingOrder[prefab.SortingOrder] = Instantiate(panelSettings);
                }
            }

            foreach (var (sortingOrder, panelSetting) in panelSettingsBySortingOrder)
            {
                panelSetting.name += $"({sortingOrder.ToString()})";
                panelSetting.sortingOrder = sortingOrder;
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

        public UIPanelLanguageConfig GetLanguageConfig(string localeCode)
        {
            foreach (var config in languageConfigs)
            {
                if (config.LocaleCode == localeCode)
                {
                    return config;
                }
            }

            return null;
        }
#if UNITY_EDITOR
        string INameOwner.Name => "UI Panel";
        EditorIcon IEditorIconProvider.Icon => SdfIconType.LayoutWtf;
#endif
    }
}
