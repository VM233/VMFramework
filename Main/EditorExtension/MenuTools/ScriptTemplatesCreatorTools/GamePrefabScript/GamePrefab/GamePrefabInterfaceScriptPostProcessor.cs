#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public sealed class GamePrefabInterfaceScriptPostProcessor
        : ScriptCreationPostProcessor<GamePrefabInterfaceScriptExtraInfo>
    {
        protected override void PostProcess(string scriptAbsolutePath, ref string scriptContent,
            GamePrefabInterfaceScriptExtraInfo extraInfo)
        {
            Replace(ref scriptContent, "PARENT_INTERFACE_NAME", extraInfo.parentInterfaceName);
        }
    }
}
#endif