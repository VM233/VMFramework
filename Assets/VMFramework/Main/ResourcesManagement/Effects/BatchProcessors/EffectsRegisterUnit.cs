#if UNITY_EDITOR
using UnityEngine;
using VMFramework.Core;
using VMFramework.Editor.BatchProcessor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.ResourcesManagement
{
    internal sealed class EffectsRegisterUnit : GamePrefabRegisterUnit<GameObject>
    {
        protected override string ProcessButtonName => "Register Effects";

        protected override bool IsAsset(object obj, out GameObject gameObject)
        {
            gameObject = null;

            if (obj is not GameObject go)
            {
                return false;
            }

            if (go.IsPrefabAsset() == false)
            {
                return false;
            }

            gameObject = go;

            return true;
        }

        protected override IGamePrefab OnProcessAsset(GameObject gameObject)
        {
            if (checkUnique)
            {
                foreach (var visualEffect in GamePrefabManager.GetAllGamePrefabs<IEffectConfig>())
                {
                    if (visualEffect is IPrefabProvider prefabProvider && prefabProvider.Prefab == gameObject)
                    {
                        Debugger.LogWarning($"To prevent duplicates, {gameObject.name} has not been imported.");
                        return null;
                    }
                }
            }

            var generalVisualEffect = new EffectConfig()
            {
                id = gameObject.name.ToSnakeCase(),
                prefab = gameObject,
            };
            
            return generalVisualEffect;
        }
    }
}
#endif