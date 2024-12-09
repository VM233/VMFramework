using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Containers
{
    public static class ContainerItemUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryMergeWith(this IContainerItem item, IContainerItem other)
        {
            if (item.IsMergeableWith(other))
            {
                item.MergeWith(other);

                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySplit(this IContainerItem item, int targetCount, out IContainerItem result)
        {
            result = null;

            if (item.IsSplittable(targetCount))
            {
                result = item.Split(targetCount);

                return true;
            }

            return false;
        }
    }
}