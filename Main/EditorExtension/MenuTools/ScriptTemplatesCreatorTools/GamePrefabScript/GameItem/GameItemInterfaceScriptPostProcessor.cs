#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public sealed class GameItemInterfaceScriptPostProcessor
        : ScriptCreationPostProcessor<GameItemInterfaceScriptExtraInfo>
    {
        protected override void PostProcess(string scriptAbsolutePath, ref string scriptContent,
            GameItemInterfaceScriptExtraInfo extraInfo)
        {
            Replace(ref scriptContent, "PARENT_INTERFACE_NAME", extraInfo.parentInterfaceName);
        }
    }
}
#endif