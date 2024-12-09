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
    }
}