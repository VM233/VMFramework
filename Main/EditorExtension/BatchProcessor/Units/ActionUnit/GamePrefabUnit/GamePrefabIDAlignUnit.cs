#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Editor.BatchProcessor
{
    public class GamePrefabIDAlignUnit : SingleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonName => "Align ID with Name";

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            foreach (var selectedObject in selectedObjects)
            {
                if (selectedObject is not Object asset)
                {
                    continue;
                }

                if (selectedObject is not IGamePrefabsProvider gamePrefabsProvider)
                {
                    selectedObject.TryGetComponent(out gamePrefabsProvider);
                }

                if (gamePrefabsProvider == null)
                {
                    continue;
                }

                var gamePrefabs = ListPool<IGamePrefab>.Default.Get();
                gamePrefabs.Clear();

                if (gamePrefabs.Count > 1)
                {
                    gamePrefabs.ReturnToDefaultPool();
                    continue;
                }

                gamePrefabsProvider.GetGamePrefabs(gamePrefabs);

                foreach (var gamePrefab in gamePrefabs)
                {
                    var id = asset.name.MakeWordsSuffix(gamePrefab.IDSuffix).ToSnakeCase();

                    if (id != gamePrefab.id)
                    {
                        gamePrefabs.ReturnToDefaultPool();
                        return true;
                    }
                }

                gamePrefabs.ReturnToDefaultPool();
            }

            return false;
        }

        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            foreach (var selectedObject in selectedObjects)
            {
                if (selectedObject is not Object asset)
                {
                    continue;
                }

                if (selectedObject is not IGamePrefabsProvider gamePrefabsProvider)
                {
                    selectedObject.TryGetComponent(out gamePrefabsProvider);
                }

                if (gamePrefabsProvider == null)
                {
                    continue;
                }

                var gamePrefabs = ListPool<IGamePrefab>.Default.Get();
                gamePrefabs.Clear();

                if (gamePrefabs.Count > 1)
                {
                    gamePrefabs.ReturnToDefaultPool();
                    continue;
                }

                gamePrefabsProvider.GetGamePrefabs(gamePrefabs);

                foreach (var gamePrefab in gamePrefabs)
                {
                    gamePrefab.id = asset.name.MakeWordsSuffix(gamePrefab.IDSuffix).ToSnakeCase();
                }

                gamePrefabs.ReturnToDefaultPool();

                asset.SetEditorDirty();
            }

            return selectedObjects;
        }
    }
}
#endif