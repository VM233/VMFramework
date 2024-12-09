using System.Runtime.CompilerServices;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Containers
{
    public static class ContainerItemFactory
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IContainerItem Create(string id, int count)
        {
            var newItem = GameItemManager.Get<IContainerItem>(id);
            newItem.Count = count;
            return newItem;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TItem Create<TItem>(string id, int count) where TItem : IContainerItem
        {
            var newItem = GameItemManager.Get<TItem>(id);
            newItem.Count = count;
            return newItem;
        }
    }
}