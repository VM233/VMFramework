#if FISHNET
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;

namespace VMFramework.Network 
{
    [ManagerCreationProvider(ManagerType.SettingCore)]
    public sealed partial class NetworkSetting : GlobalSetting<NetworkSetting, NetworkSettingFile>
    {
        public static DefaultGlobalScenesGeneralSetting DefaultGlobalScenesGeneralSetting =>
            GlobalSettingFile.defaultGlobalScenesGeneralSetting;
    }
}
#endif