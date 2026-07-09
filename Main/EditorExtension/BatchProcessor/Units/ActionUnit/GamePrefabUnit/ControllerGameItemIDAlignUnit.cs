#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.Editor.BatchProcessor
{
    public class ControllerGameItemIDAlignUnit : SingleButtonBatchProcessorUnit
    {
        protected override string ProcessButtonName => "Align ID with Name";

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            foreach (var selectedObject in selectedObjects)
            {
                if (selectedObject is not GameObject gameObject)
                {
                    continue;
                }

                if (gameObject.TryGetComponent(out IGameItem _) == false)
                {
                    continue;
                }

                IGamePrefab gamePrefab = null;

                foreach (var prefabProvider in GamePrefabManager.GetAllGamePrefabs<IPrefabProvider>())
                {
                    if (prefabProvider.Prefab == gameObject)
                    {
                        gamePrefab = (IGamePrefab)prefabProvider;
                        break;
                    }
                }

                if (gamePrefab == null)
                {
                    continue;
                }

                var id = gameObject.name.MakeWordsSuffix(gamePrefab.IDSuffix).ToSnakeCase();

                if (id != gamePrefab.id)
                {
                    return true;
                }
            }

            return false;
        }

        protected override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            foreach (var selectedObject in selectedObjects)
            {
                if (selectedObject is not GameObject gameObject)
                {
                    continue;
                }

                if (gameObject.TryGetComponent(out IGameItem _) == false)
                {
                    continue;
                }

                IGamePrefab gamePrefab = null;

                foreach (var prefabProvider in GamePrefabManager.GetAllGamePrefabs<IPrefabProvider>())
                {
                    if (prefabProvider.Prefab == gameObject)
                    {
                        gamePrefab = (IGamePrefab)prefabProvider;
                        break;
                    }
                }

                if (gamePrefab == null)
                {
                    continue;
                }

                var id = gameObject.name.MakeWordsSuffix(gamePrefab.IDSuffix).ToSnakeCase();

                if (id == gamePrefab.id)
                {
                    continue;
                }
                
                var oldID = gamePrefab.id;

                var wrapper = GamePrefabWrapperQueryTools.GetGamePrefabWrapper(oldID);

                gamePrefab.id = id;
                
                wrapper.EnforceSave();

                if (wrapper != null)
                {
                    var targetName = id.ToPascalCase();

                    if (wrapper.name != targetName)
                    {
                        wrapper.Rename(targetName);
                        wrapper.SetEditorDirty();
                    }
                }
            }

            return selectedObjects;
        }
    }
}
#endif