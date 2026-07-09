#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public sealed class GameItemSerializerScriptExtraInfo : ScriptCreationExtraInfo
    {
        public string GameItemName { get; init; }
        
        public string GameItemInterfaceName { get; init; }
        
        public string GameItemFieldName { get; init; }
    }
}
#endif