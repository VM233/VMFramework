#if FISHNET
using Sirenix.OdinInspector;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;

namespace VMFramework.Network 
{
    [GlobalSettingFileConfig(FileName = nameof(NetworkSettingFile))]
    public sealed partial class NetworkSettingFile : GlobalSettingFile
    {
        private const string NETWORK_CATEGORY = "Network";
        
        [TabGroup(TAB_GROUP_NAME, NETWORK_CATEGORY)]
        public DefaultGlobalScenesGeneralSetting defaultGlobalScenesGeneralSetting;
    }
}
#endif