#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.Editor.GameEditor
{
    public class GamePrefabProviderToolbarButtonProcessor : TypedFuncProcessor<IGamePrefabsProvider, ToolbarButtonConfig>
    {
        protected override void ProcessTypedTarget(IGamePrefabsProvider target, ICollection<ToolbarButtonConfig> results)
        {
            results.Add(new(EditorNames.OPEN_GAME_PREFAB_SCRIPT_PATH, target.OpenGamePrefabScripts));
            results.Add(new(EditorNames.OPEN_GAME_ITEM_SCRIPT_PATH, target.OpenGameItemScripts));
        }
    }
}
#endif