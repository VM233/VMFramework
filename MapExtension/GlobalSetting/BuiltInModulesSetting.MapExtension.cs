using VMFramework.Maps;

namespace VMFramework.GameLogicArchitecture
{
    public partial class BuiltInModulesSetting
    {
        public static GridMapGeneralSetting GridMapGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.gridMapGeneralSetting;
        
        public static TileBaseConfigGeneralSetting TileBaseConfigGeneralSetting =>
            GlobalSettingFile == null ? null : GlobalSettingFile.tileBaseConfigGeneralSetting;
    }
}