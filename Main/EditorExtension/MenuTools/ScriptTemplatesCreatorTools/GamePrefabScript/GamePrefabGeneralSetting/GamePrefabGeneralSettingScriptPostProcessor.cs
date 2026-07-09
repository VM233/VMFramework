#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public sealed class GamePrefabGeneralSettingScriptPostProcessor
        : ScriptCreationPostProcessor<GamePrefabGeneralSettingScriptExtraInfo>
    {
        protected override void PostProcess(string scriptAbsolutePath, ref string scriptContent,
            GamePrefabGeneralSettingScriptExtraInfo extraInfo)
        {
            Replace(ref scriptContent, "BASE_GAME_PREFAB_TYPE", extraInfo.BaseGamePrefabType);
            Replace(ref scriptContent, "NAME_IN_GAME_EDITOR", extraInfo.NameInGameEditor);
            
            Region(ref scriptContent, "GAME_ITEM_NAME_OVERRIDE_REGION", extraInfo.EnableGameItemNameOverrideRegion);
            Replace(ref scriptContent, "GAME_ITEM_NAME", extraInfo.GameItemName);
        }
    }
}
#endif