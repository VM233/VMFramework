using System.Collections.Generic;
using VMFramework.Core.Pools;

namespace VMFramework.Core
{
    public interface IAvailableItemsProvider
    {
        public IEnumerable<object> GetAvailableObjects();
    }

    public interface IAvailableItemsProvider<TItem> : IAvailableItemsProvider
    {
        public void GetAvailableItems(ICollection<TItem> items);

        public IEnumerable<TItem> GetAvailableItems()
        {
            var items = ListPool<TItem>.Default.Get();
            items.Clear();
            GetAvailableItems(items);
            foreach (var item in items)
            {
                yield return item;
            }

            items.ReturnToDefaultPool();
        }

        IEnumerable<object> IAvailableItemsProvider.GetAvailableObjects()
        {
            var items = ListPool<TItem>.Default.Get();
            items.Clear();
            GetAvailableItems(items);
            foreach (var item in items)
            {
                yield return item;
            }

            items.ReturnToDefaultPool();
        }
    }
}