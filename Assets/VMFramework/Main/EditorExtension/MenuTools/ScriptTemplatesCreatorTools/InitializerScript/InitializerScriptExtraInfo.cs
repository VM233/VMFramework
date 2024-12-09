#if UNITY_EDITOR
using VMFramework.Core.Editor;
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public class InitializerScriptExtraInfo : ScriptCreationExtraInfo
    {
        public string initializationOrderName { get; init; }
    }
}
#endif