#if UNITY_EDITOR
using Sirenix.OdinInspector;

namespace VMFramework.Editor
{
    public sealed class GlobalSettingScriptCreationViewer : ScriptCreationViewer
    {
        [EnumToggleButtons]
        public GlobalSettingFileFolderType folderType;
    }
}
#endif