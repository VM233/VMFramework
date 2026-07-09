using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public interface IPrefabConfig : IPrefabProvider
    {
        public void SetPrefab(GameObject prefab);
    }
}