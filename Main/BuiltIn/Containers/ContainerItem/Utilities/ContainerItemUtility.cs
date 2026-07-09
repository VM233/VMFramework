using System.Runtime.CompilerServices;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Containers
{
    public static class ContainerItemUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReturnIfNoOwner<TContainerItem>(this TContainerItem item)
            where TContainerItem : IContainerItem
        {
            if (item.IsDestroyed)
            {
                return;
            }

            if (item.Owner.GetValue() == null)
            {
                GameItemManager.Instance.Return(item);
            }
        }
    }
}