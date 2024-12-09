using UnityEngine;

namespace VMFramework.Core
{
    public interface IController
    {
        public Transform transform { get; }
        
        public GameObject gameObject { get; }
        
        public T GetComponent<T>();
        
        public bool TryGetComponent<T>(out T component);
    }
}