#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public sealed class GameItemScriptExtraInfo : ScriptCreationExtraInfo
    {
        public string parentClassName { get; init; }
        
        public bool enableParentInterfaceRegion { get; init; }
        
        public string parentInterfaceName { get; init; }
        
        public string gamePrefabInterfaceName { get; init; }
        
        public string gamePrefabFieldName { get; init; }
    }
}
#endif