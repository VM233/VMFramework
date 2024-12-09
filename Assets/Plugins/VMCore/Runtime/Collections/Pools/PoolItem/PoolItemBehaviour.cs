using UnityEngine;

namespace VMFramework.Core.Pools
{
    public class PoolItemBehaviour : MonoBehaviour, IPoolItem
    {
        protected virtual Transform CachedContainer => null;
        
        protected virtual void OnGet()
        {
            gameObject.SetActive(true);
        }

        protected virtual void OnReturn()
        {
            gameObject.SetActive(false);
            transform.SetParent(CachedContainer);
        }

        protected virtual void OnClear()
        {
            Destroy(gameObject);
        }

        void IPoolItem.OnGet()
        {
            OnGet();
        }

        void IPoolItem.OnReturn()
        {
            OnReturn();
        }

        void IPoolItem.OnClear()
        {
            OnClear();
        }
    }
}