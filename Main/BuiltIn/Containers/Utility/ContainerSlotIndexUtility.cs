using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Containers
{
    public static class ContainerSlotIndexUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int minSlotIndex, int maxSlotIndex) ClampSlotRange<TContainer>(this TContainer container,
            int minSlotIndex, int maxSlotIndex)
            where TContainer : IContainer
        {
            return (minSlotIndex.Max(0), maxSlotIndex.Min(container.Count - 1));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeInteger ClampSlotRange(this IContainer container, RangeInteger slotRange)
        {
            slotRange.min = slotRange.min.Max(0);
            slotRange.max = slotRange.max.Min(container.Count - 1);
            return slotRange;
        }
    }
}