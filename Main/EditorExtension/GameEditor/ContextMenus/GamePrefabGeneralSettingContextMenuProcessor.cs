#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using VMFramework.Configuration;
using VMFramework.Core.Linq;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.Editor.GameEditor
{
    public class GamePrefabGeneralSettingContextMenuProcessor
        : TypedFuncTargetsProcessor<GamePrefabGeneralSetting, ContextMenuItemConfig>
    {
        protected override void ProcessTypedTargets(IReadOnlyList<GamePrefabGeneralSetting> targets,
            ICollection<ContextMenuItemConfig> results)
        {
            results.Add(new(EditorNames.OPEN_GAME_PREFAB_SCRIPT_PATH,
                () => { targets.Examine(item => item.OpenGamePrefabScript()); }));
            results.Add(new(EditorNames.OPEN_INITIAL_GAME_PREFABS_SCRIPTS_PATH,
                () => { targets.Examine(item => item.OpenInitialGamePrefabScripts()); }));

            if (targets.Any(item => item.initialGamePrefabProviders.HasAnyGameItem()))
            {
                results.Add(new(EditorNames.OPEN_GAME_ITEMS_OF_INITIAL_GAME_PREFABS_SCRIPTS_PATH,
                    () => { targets.Examine(item => item.OpenGameItemsOfInitialGamePrefabsScripts()); }));
            }

            results.Add(new(EditorNames.SAVE_ALL, () => { targets.Examine(item => item.SaveAllGamePrefabs()); }));
        }
    }
}
#endif