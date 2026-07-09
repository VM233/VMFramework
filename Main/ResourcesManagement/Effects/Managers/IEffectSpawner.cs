using UnityEngine;

namespace VMFramework.Effects
{
    public interface IEffectSpawner
    {
        public Transform Container { get; }

        public IEffect Spawn(string id, Vector3 position, bool isWorldSpace, Transform parent = null);
    }
}