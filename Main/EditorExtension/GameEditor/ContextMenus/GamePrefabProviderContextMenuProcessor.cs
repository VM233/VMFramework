#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.Editor.GameEditor
{
    public class GamePrefabProviderContextMenuProcessor
        : TypedFuncTargetsProcessor<IGamePrefabsProvider, ContextMenuItemConfig>
    {
        protected override void ProcessTypedTargets(IReadOnlyList<IGamePrefabsProvider> targets,
            ICollection<ContextMenuItemConfig> results)
        {
            results.Add(new(EditorNames.OPEN_GAME_PREFAB_SCRIPT_PATH, targets.OpenGamePrefabScripts));
            results.Add(new(EditorNames.OPEN_GAME_ITEM_SCRIPT_PATH, targets.OpenGameItemScripts));
        }
    }
}
#endif