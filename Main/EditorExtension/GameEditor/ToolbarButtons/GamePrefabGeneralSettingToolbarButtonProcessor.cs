#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.Editor.GameEditor
{
    public class GamePrefabGeneralSettingToolbarButtonProcessor
        : TypedFuncProcessor<GamePrefabGeneralSetting, ToolbarButtonConfig>
    {
        protected override void ProcessTypedTarget(GamePrefabGeneralSetting target,
            ICollection<ToolbarButtonConfig> results)
        {
            results.Add(new(EditorNames.OPEN_GAME_PREFAB_SCRIPT_PATH, target.OpenGamePrefabScript));
            results.Add(new(EditorNames.OPEN_INITIAL_GAME_PREFABS_SCRIPTS_PATH, target.OpenInitialGamePrefabScripts));

            if (target.initialGamePrefabProviders.HasAnyGameItem())
            {
                results.Add(new(EditorNames.OPEN_GAME_ITEMS_OF_INITIAL_GAME_PREFABS_SCRIPTS_PATH,
                    target.OpenGameItemsOfInitialGamePrefabsScripts));
            }

            results.Add(new(EditorNames.SAVE_ALL, target.SaveAllGamePrefabs));
        }
    }
}
#endif