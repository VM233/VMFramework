#if UNITY_EDITOR && FISHNET
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.Network 
{
    [GlobalSettingFileEditorConfig(FolderPath = ConfigurationPath.INTERNAL_GLOBAL_SETTINGS_PATH)]
    public partial class NetworkSettingFile
    {
        
    }
}
#endif