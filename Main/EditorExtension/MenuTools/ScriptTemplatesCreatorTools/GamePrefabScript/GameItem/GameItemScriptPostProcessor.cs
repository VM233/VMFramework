#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public sealed class GameItemScriptPostProcessor : ScriptCreationPostProcessor<GameItemScriptExtraInfo>
    {
        protected override void PostProcess(string scriptAbsolutePath, ref string scriptContent,
            GameItemScriptExtraInfo extraInfo)
        {
            Replace(ref scriptContent, "PARENT_CLASS_NAME", extraInfo.parentClassName);
            
            Region(ref scriptContent, "PARENT_INTERFACE_REGION", extraInfo.enableParentInterfaceRegion);
            Replace(ref scriptContent, "PARENT_INTERFACE_NAME", extraInfo.parentInterfaceName);
            
            Replace(ref scriptContent, "GAME_PREFAB_INTERFACE_NAME", extraInfo.gamePrefabInterfaceName);
            Replace(ref scriptContent, "GAME_PREFAB_FIELD_NAME", extraInfo.gamePrefabFieldName);
        }
    }
}
#endif