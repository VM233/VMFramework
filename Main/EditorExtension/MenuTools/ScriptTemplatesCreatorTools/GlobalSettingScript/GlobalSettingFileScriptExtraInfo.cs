#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public sealed class GlobalSettingFileScriptExtraInfo : ScriptCreationExtraInfo
    {
        public string folderPath { get; init; }
        
        public string nameInGameEditor { get; init; }
    }
}
#endif