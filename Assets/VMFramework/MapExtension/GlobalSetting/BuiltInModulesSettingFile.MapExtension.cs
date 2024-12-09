﻿using Sirenix.OdinInspector;
using VMFramework.Maps;

namespace VMFramework.GameLogicArchitecture
{
    public partial class BuiltInModulesSettingFile
    {
        [TabGroup(TAB_GROUP_NAME, BUILTIN_MODULE_CATEGORY)]
        [Required]
        public GridMapGeneralSetting gridMapGeneralSetting;
        
        [TabGroup(TAB_GROUP_NAME, BUILTIN_MODULE_CATEGORY)]
        [Required]
        public ExtendedRuleTileGeneralSetting extendedRuleTileGeneralSetting;
        
        [TabGroup(TAB_GROUP_NAME, BUILTIN_MODULE_CATEGORY)]
        [Required]
        public TileBaseConfigGeneralSetting tileBaseConfigGeneralSetting;
        
        [TabGroup(TAB_GROUP_NAME, BUILTIN_MODULE_CATEGORY)]
        [Required]
        public ShadowCaster2DGeneralSetting shadowCaster2DGeneralSetting;
    }
}