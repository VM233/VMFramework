using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Tools
{
    public class PrefabController<TPrefab> : MonoBehaviour, IPrefabController<TPrefab> where TPrefab : Component
    {
        [SerializeField]
        [Required]
        private TPrefab initialPrefab;
        
        [ShowInInspector]
        [ReadOnly]
        private TPrefab _prefab;

        public TPrefab Prefab
        {
            get
            {
                if (_prefab == null)
                {
                    SetPrefab(initialPrefab);
                }
                
                return _prefab;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SetPrefab(TPrefab newPrefab)
        {
            if (newPrefab == null)
            {
                Debug.LogError($"{nameof(newPrefab)} is null");
                return;
            }

            _prefab = newPrefab;

            newPrefab.SetActive(true);
        }
    }
}