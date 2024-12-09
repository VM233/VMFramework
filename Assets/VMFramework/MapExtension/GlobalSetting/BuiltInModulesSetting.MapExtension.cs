using VMFramework.Maps;

namespace VMFramework.GameLogicArchitecture
{
    public partial class BuiltInModulesSetting
    {
        public static GridMapGeneralSetting GridMapGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.gridMapGeneralSetting;
        
        public static ExtendedRuleTileGeneralSetting ExtendedRuleTileGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.extendedRuleTileGeneralSetting;
        
        public static TileBaseConfigGeneralSetting TileBaseConfigGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.tileBaseConfigGeneralSetting;

        public static ShadowCaster2DGeneralSetting ShadowCaster2DGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.shadowCaster2DGeneralSetting;
    }
}