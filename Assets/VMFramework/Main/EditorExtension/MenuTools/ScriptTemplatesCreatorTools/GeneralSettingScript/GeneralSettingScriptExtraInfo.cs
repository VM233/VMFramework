#if UNITY_EDITOR
using VMFramework.Core.Editor;
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public sealed class GeneralSettingScriptExtraInfo : ScriptCreationExtraInfo
    {
        public string nameInGameEditor { get; init; }
    }
}
#endif