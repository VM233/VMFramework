#if UNITY_EDITOR
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
        }
    }
}
#endif