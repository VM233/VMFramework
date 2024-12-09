#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.Editor.BatchProcessor
{
    public abstract class GamePrefabRegisterUnit<TAsset> : GamePrefabRegisterUnit
    {
        protected virtual bool IsAsset(object obj, out TAsset asset)
        {
            if (obj is TAsset typedAsset)
            {
                asset = typedAsset;
                return true;
            }
            
            asset = default;
            return false;
        }
        
        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return base.IsValid(selectedObjects) && selectedObjects.Any(obj => IsAsset(obj, out _));
        }

        protected sealed override void OnProcess(IReadOnlyList<object> selectedObjects, List<IGamePrefab> gamePrefabs)
        {
            foreach (var obj in selectedObjects)
            {
                if (IsAsset(obj, out var asset))
                {
                    var gamePrefab = OnProcessAsset(asset);
                    gamePrefabs.AddIfNotNull(gamePrefab);
                }
            }
        }

        protected abstract IGamePrefab OnProcessAsset(TAsset asset);
    }
    
    public abstract class GamePrefabRegisterUnit : SingleButtonBatchProcessorUnit
    {
        [LabelWidth(100), HorizontalGroup]
        [SerializeField]
        protected bool checkUnique = true;

        [LabelWidth(150), HorizontalGroup]
        [SerializeField]
        private bool combineGamePrefabs = false;

        [LabelWidth(180)]
        [SerializeField]
        [ShowIf(nameof(combineGamePrefabs))]
        private string combinedGamePrefabName;
        
        private readonly List<IGamePrefab> gamePrefabs = new();

        public override bool IsValid(IReadOnlyList<object> selectedObjects)
        {
            return EditorApplication.isPlaying == false;
        }

        protected sealed override IEnumerable<object> OnProcess(IReadOnlyList<object> selectedObjects)
        {
            if (combineGamePrefabs)
            {
                if (combinedGamePrefabName.IsNullOrWhiteSpace())
                {
                    Debugger.LogError("Combined GamePrefab name cannot be empty");
                    return selectedObjects;
                }
            }
            
            gamePrefabs.Clear();
            
            OnProcess(selectedObjects, gamePrefabs);
            
            if (gamePrefabs.Count == 0)
            {
                return selectedObjects;
            }

            if (combineGamePrefabs)
            {
                GamePrefabWrapperCreator.CreateGamePrefabWrapper(combinedGamePrefabName, gamePrefabs.ToArray());
            }
            else
            {
                foreach (var config in gamePrefabs)
                {
                    GamePrefabWrapperCreator.CreateGamePrefabWrapper(config, GamePrefabWrapperType.Single);
                }
            }

            return selectedObjects;
        }

        protected abstract void OnProcess(IReadOnlyList<object> selectedObjects, List<IGamePrefab> gamePrefabs);
    }
}
#endif