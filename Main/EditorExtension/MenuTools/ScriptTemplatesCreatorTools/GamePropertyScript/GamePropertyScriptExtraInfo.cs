#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public sealed class GamePropertyScriptExtraInfo : ScriptCreationExtraInfo
    {
        public string GamePropertyID { get; init; }
        
        public string TargetType { get; init; }
        
        public string TargetName { get; init; }
    }
}
#endif