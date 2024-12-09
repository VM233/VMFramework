#if UNITY_EDITOR
using UnityEngine;
using VMFramework.Core;
using VMFramework.Editor.BatchProcessor;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.ResourcesManagement
{
    internal sealed class EffectsUnregisterUnit : GamePrefabUnregisterUnit<GameObject>
    {
        protected override string ProcessButtonName => "Unregister Effects";

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

        protected override void OnProcessAsset(GameObject gameObject)
        {
            GamePrefabWrapperRemover.RemoveGamePrefabWrapperWhere<IEffectConfig>(visualEffect =>
                visualEffect is IPrefabProvider prefabProvider && prefabProvider.Prefab == gameObject);
        }
    }
}
#endif