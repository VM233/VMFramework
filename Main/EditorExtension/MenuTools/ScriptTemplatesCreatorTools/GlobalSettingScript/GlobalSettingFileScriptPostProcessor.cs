#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public sealed class GlobalSettingFileScriptPostProcessor
        : ScriptCreationPostProcessor<GlobalSettingFileScriptExtraInfo>
    {
        protected override void PostProcess(string scriptAbsolutePath, ref string scriptContent,
            GlobalSettingFileScriptExtraInfo extraInfo)
        {
            Replace(ref scriptContent, "FOLDER_PATH", extraInfo.folderPath);
            Replace(ref scriptContent, "NAME_IN_GAME_EDITOR", extraInfo.nameInGameEditor);
        }
    }
}
#endif