using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Containers
{
    public static partial class ContainerUtility
    {
        #region Split

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySplitItem(this IContainer container, int slotIndex, int count, out IContainerItem splitResult)
        {
            var item = container.GetItem(slotIndex);

            if (item.IsSplittable(count) == false)
            {
                splitResult = null;
                return false;
            }

            splitResult = item.Split(count);
            return true;
        }

        public static bool TrySplitItemTo(this IContainer container, int slotIndex, int count,
            IContainer targetContainer, int targetSlotIndex)
        {
            var item = container.GetItem(slotIndex);

            if (item.IsSplittable(count))
            {
                var targetItem = targetContainer.GetItem(targetSlotIndex);

                var splitResult = item.Split(count);

                if (targetItem == null)
                {
                    targetContainer.SetItem(targetSlotIndex, splitResult);

                    return true;
                }

                if (targetItem.IsMergeableWith(splitResult))
                {
                    targetItem.MergeWith(splitResult);
                    item.MergeWith(splitResult);

                    return true;
                }

                item.MergeWith(splitResult);
            }

            return false;
        }

        #endregion
    }
}
