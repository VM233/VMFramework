#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public class InitializerScriptExtraInfo : ScriptCreationExtraInfo
    {
        public string initializationOrderName { get; init; }
    }
}
#endif