#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public sealed class GamePrefabInterfaceScriptExtraInfo : ScriptCreationExtraInfo
    {
        public string parentInterfaceName { get; init; }
    }
}
#endif