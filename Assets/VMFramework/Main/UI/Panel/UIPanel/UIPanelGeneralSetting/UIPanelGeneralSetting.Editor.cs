#if UNITY_EDITOR
using VMFramework.Core;

namespace VMFramework.UI
{
    public partial class UIPanelGeneralSetting
    {
        // private const string DEFAULT_THEME_STYLE_SHEET_NAME = "UnityDefaultRuntimeTheme";
        
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            // if (defaultTheme == null)
            // {
            //     defaultTheme = DEFAULT_THEME_STYLE_SHEET_NAME.FindAssetOfName<ThemeStyleSheet>();
            // }

            languageConfigs ??= new();
            sortingOrderPresets ??= new();

            sortingOrderPresets.TryAddConfigEditor(new PriorityPreset()
            {
                presetID = DEFAULT_SORTING_ORDER_ID,
                priority = DEFAULT_SORTING_ORDER
            });

            sortingOrderPresets.TryAddConfigEditor(new PriorityPreset()
            {
                presetID = DEBUG_SORTING_ORDER_ID,
                priority = DEBUG_SORTING_ORDER
            });
        }
    }
}
#endif