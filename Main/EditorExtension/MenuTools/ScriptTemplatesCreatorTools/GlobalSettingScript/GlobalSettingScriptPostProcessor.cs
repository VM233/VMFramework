#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public sealed class GlobalSettingScriptPostProcessor : ScriptCreationPostProcessor<GlobalSettingScriptExtraInfo>
    {
        protected override void PostProcess(string scriptAbsolutePath, ref string scriptContent,
            GlobalSettingScriptExtraInfo extraInfo)
        {
            Replace(ref scriptContent, "GLOBAL_SETTING_FILE_NAME", extraInfo.globalSettingFileName);
        }
    }
}
#endif