#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.OdinExtensions;

namespace VMFramework.Effects
{
    public partial class EffectSpawner
    {
        [Button]
        private void _Spawn([HideLabel] [GamePrefabID(typeof(IEffectConfig))] string id, [HideLabel] Vector3 position,
            bool isWorldSpace, Transform parent = null)
        {
            Spawn(id, position, isWorldSpace, parent);
        }
    }
}
#endif