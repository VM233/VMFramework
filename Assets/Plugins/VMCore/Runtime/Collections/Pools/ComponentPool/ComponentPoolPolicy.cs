using UnityEngine;

namespace VMFramework.Core.Pools
{
    public abstract class ComponentPoolPolicy<TComponent> : PoolPolicy<TComponent> where TComponent : Component
    {
        public override TComponent PreGet(TComponent item)
        {
            item.gameObject.SetActive(true);
            return item;
        }

        public override bool Return(TComponent item)
        {
            item.SetActive(false);
            return true;
        }

        public override void Clear(TComponent item)
        {
            Object.Destroy(item.gameObject);
        }
    }
}