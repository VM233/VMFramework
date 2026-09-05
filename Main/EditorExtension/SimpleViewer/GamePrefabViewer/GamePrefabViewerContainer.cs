#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Editor
{
    internal class GamePrefabViewerContainer : SimpleOdinEditorWindowContainer
    {
        [Searchable]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        [ShowInInspector]
        public IEnumerable<IGamePrefab> gamePrefabs => GamePrefabManager.GetAllGamePrefabs();

        [Searchable]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        [ShowInInspector]
        public IEnumerable<string> gamePrefabIDs => GamePrefabManager.GetAllIDs();
    }
}
#endif
