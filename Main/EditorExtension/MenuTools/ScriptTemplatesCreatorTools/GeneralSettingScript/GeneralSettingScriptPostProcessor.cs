#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public sealed class GeneralSettingScriptPostProcessor : ScriptCreationPostProcessor<GeneralSettingScriptExtraInfo>
    {
        protected override void PostProcess(string scriptAbsolutePath, ref string scriptContent,
            GeneralSettingScriptExtraInfo extraInfo)
        {
            Replace(ref scriptContent, "NAME_IN_GAME_EDITOR", extraInfo.nameInGameEditor);
        }
    }
}
#endif