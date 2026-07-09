#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public sealed class GamePrefabScriptExtraInfo : ScriptCreationExtraInfo
    {
        public string ParentClassName { get; init; }
        
        public bool EnableParentInterfaceRegion { get; init; }
        
        public string ParentInterfaceName { get; init; }
        
        public bool EnableIDSuffixOverrideRegion { get; init; }
        
        public string IDSuffix { get; init; }
        
        public bool EnableGameItemTypeOverrideRegion { get; init; }
        
        public string GameItemType { get; init; }
    }
}
#endif