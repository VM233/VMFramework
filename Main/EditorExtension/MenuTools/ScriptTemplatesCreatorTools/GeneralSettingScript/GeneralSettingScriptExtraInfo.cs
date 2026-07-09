#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public sealed class GeneralSettingScriptExtraInfo : ScriptCreationExtraInfo
    {
        public string nameInGameEditor { get; init; }
    }
}
#endif