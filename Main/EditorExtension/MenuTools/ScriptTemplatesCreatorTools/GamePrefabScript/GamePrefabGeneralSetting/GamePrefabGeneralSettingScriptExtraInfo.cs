#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public sealed class GamePrefabGeneralSettingScriptExtraInfo : ScriptCreationExtraInfo
    {
        public string BaseGamePrefabType { get; init; }
        
        public string NameInGameEditor { get; init; }
        
        public bool EnableGameItemNameOverrideRegion { get; init; }
        
        public string GameItemName { get; init; }
    }
}
#endif