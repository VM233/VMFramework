#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public sealed class GamePrefabScriptPostProcessor
        : ScriptCreationPostProcessor<GamePrefabScriptExtraInfo>
    {
        protected override void PostProcess(string scriptAbsolutePath, ref string scriptContent,
            GamePrefabScriptExtraInfo extraInfo)
        {
            Replace(ref scriptContent, "PARENT_CLASS_NAME", extraInfo.ParentClassName);
            
            Region(ref scriptContent, "PARENT_INTERFACE_REGION", extraInfo.EnableParentInterfaceRegion);
            Replace(ref scriptContent, "PARENT_INTERFACE_NAME", extraInfo.ParentInterfaceName);
            
            Region(ref scriptContent, "ID_SUFFIX_OVERRIDE_REGION", extraInfo.EnableIDSuffixOverrideRegion);
            Replace(ref scriptContent, "ID_SUFFIX", extraInfo.IDSuffix);
            
            Region(ref scriptContent, "GAME_ITEM_TYPE_OVERRIDE_REGION", extraInfo.EnableGameItemTypeOverrideRegion);
            Replace(ref scriptContent, "GAME_ITEM_TYPE", extraInfo.GameItemType);
        }
    }
}
#endif