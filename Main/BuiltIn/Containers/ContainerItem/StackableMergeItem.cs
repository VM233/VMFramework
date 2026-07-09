using System.Collections.Generic;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Containers
{
    public class StackableMergeItem : MonoBehaviour
    {
        public delegate void MergeHandler(IContainerItem item, IContainerItem other, ref int newCount,
            ref int otherNewCount, ref int mergedCount);

        public event MergeHandler OnMergeInto;

        protected IContainerItem item;

        protected virtual void Awake()
        {
            item = GetComponentInParent<IContainerItem>();
            item.OnCheckInsertable += OnCheckInsertable;
            item.OnInsert += OnInsert;
            item.OnSplit += OnSplit;
            item.OnRemove += OnRemove;
        }

        protected virtual void OnCheckInsertable(IContainerItem containerItem,
            IReadOnlyCollection<ContainerItemIndexPair> others, InsertEmptyRangeInfo emptyRangeInfo,
            ICollection<int> filledSlots, int preferredCount, ref ItemMergeResult? mergeResult,
            ref int actualInsertCount)
        {
            if (mergeResult is not null)
            {
                return;
            }

            if (item.Count.GetValue() <= 0)
            {
                mergeResult = ItemMergeResult.FullMerge;
                return;
            }

            if (preferredCount <= 0)
            {
                mergeResult = ItemMergeResult.FullMerge;
                return;
            }

            bool anyMerge = false;
            var leftCount = item.Count.GetValue().Min(preferredCount);

            foreach (var (_, other) in others)
            {
                if (other == null)
                {
                    continue;
                }

                if (other.IsDestroyed)
                {
                    continue;
                }

                if (other.IsMergeableWith(item) == false)
                {
                    continue;
                }

                var countToMaxStack = other.MaxStackCount - other.Count.GetValue();

                if (countToMaxStack <= 0)
                {
                    continue;
                }

                var maxCountToMerge = countToMaxStack.Min(leftCount);

                leftCount -= maxCountToMerge;
                actualInsertCount += maxCountToMerge;

                if (leftCount <= 0)
                {
                    mergeResult = ItemMergeResult.FullMerge;
                    return;
                }

                anyMerge = true;
            }

            foreach (var (index, other) in others)
            {
                if (other != null)
                {
                    continue;
                }

                var maxCountToMerge = item.MaxStackCount.Min(leftCount);
                leftCount -= maxCountToMerge;
                actualInsertCount += maxCountToMerge;

                filledSlots?.Add(index);

                if (leftCount <= 0)
                {
                    mergeResult = ItemMergeResult.FullMerge;
                    return;
                }

                anyMerge = true;
            }

            if (emptyRangeInfo.range is { } emptyRange)
            {
                var existingIndices = HashSetPool<int>.Default.Get();
                existingIndices.Clear();

                foreach (var (index, _) in others)
                {
                    existingIndices.Add(index);
                }

                foreach (var index in emptyRange)
                {
                    if (existingIndices.Contains(index))
                    {
                        continue;
                    }

                    if (emptyRangeInfo.excludedIndices != null && emptyRangeInfo.excludedIndices.Contains(index))
                    {
                        continue;
                    }

                    if (emptyRangeInfo.filterMatch.IsMatchFilter(index, item) == false)
                    {
                        continue;
                    }

                    var maxCountToMerge = item.MaxStackCount.Min(leftCount);
                    leftCount -= maxCountToMerge;
                    actualInsertCount += maxCountToMerge;

                    filledSlots?.Add(index);

                    if (leftCount <= 0)
                    {
                        existingIndices.ReturnToDefaultPool();
                        mergeResult = ItemMergeResult.FullMerge;
                        return;
                    }

                    anyMerge = true;
                }

                existingIndices.ReturnToDefaultPool();
            }

            if (anyMerge)
            {
                mergeResult = ItemMergeResult.PartialMerge;
            }
            else
            {
                mergeResult = ItemMergeResult.None;
            }
        }

        protected virtual void OnInsert(IContainerItem containerItem,
            IReadOnlyCollection<ContainerItemIndexPair> others, InsertEmptyRangeInfo emptyRangeInfo,
            ICollection<ContainerItemIndexPair> splitItems, int preferredCount, ref ItemMergeResult? mergeResult,
            ref int actualInsertCount)
        {
            if (mergeResult is not null)
            {
                return;
            }

            if (item.Count.GetValue() <= 0)
            {
                mergeResult = ItemMergeResult.FullMerge;
                return;
            }

            if (preferredCount <= 0)
            {
                mergeResult = ItemMergeResult.FullMerge;
                return;
            }

            bool anyMerge = false;
            var leftCount = item.Count.GetValue().Min(preferredCount);

            foreach (var (_, other) in others)
            {
                if (other == null)
                {
                    continue;
                }

                if (other.IsDestroyed)
                {
                    continue;
                }

                if (other.IsMergeableWith(item) == false)
                {
                    continue;
                }

                var countToMaxStack = other.MaxStackCount - other.Count.GetValue();

                if (countToMaxStack <= 0)
                {
                    continue;
                }

                var maxCountToMerge = countToMaxStack.Min(leftCount);

                var newCount = item.Count.GetValue() - maxCountToMerge;
                var otherNewCount = other.Count.GetValue() + maxCountToMerge;
                OnMergeInto?.Invoke(item, other, ref newCount, ref otherNewCount, ref maxCountToMerge);

                other.Count.Value = otherNewCount;
                item.Count.Value = newCount;

                leftCount -= maxCountToMerge;
                actualInsertCount += maxCountToMerge;

                if (leftCount <= 0)
                {
                    mergeResult = ItemMergeResult.FullMerge;
                    return;
                }

                anyMerge = true;
            }

            foreach (var (index, other) in others)
            {
                if (other != null)
                {
                    continue;
                }

                if (InsertEmpty(index, ref actualInsertCount))
                {
                    mergeResult = ItemMergeResult.FullMerge;
                    return;
                }

                anyMerge = true;
            }

            if (emptyRangeInfo.range is { } emptyRange)
            {
                var existingIndices = HashSetPool<int>.Default.Get();
                existingIndices.Clear();

                foreach (var (index, _) in others)
                {
                    existingIndices.Add(index);
                }

                foreach (var index in emptyRange)
                {
                    if (existingIndices.Contains(index))
                    {
                        continue;
                    }

                    if (emptyRangeInfo.excludedIndices != null && emptyRangeInfo.excludedIndices.Contains(index))
                    {
                        continue;
                    }

                    if (emptyRangeInfo.filterMatch.IsMatchFilter(index, item) == false)
                    {
                        continue;
                    }

                    if (InsertEmpty(index, ref actualInsertCount))
                    {
                        mergeResult = ItemMergeResult.FullMerge;
                        existingIndices.ReturnToDefaultPool();
                        return;
                    }

                    anyMerge = true;
                }

                existingIndices.ReturnToDefaultPool();
            }

            if (anyMerge)
            {
                mergeResult = ItemMergeResult.PartialMerge;
            }
            else
            {
                mergeResult = ItemMergeResult.None;
            }

            return;

            bool InsertEmpty(int index, ref int actualInsertCount)
            {
                var maxCountToMerge = item.MaxStackCount.Min(leftCount);
                var count = item.Count.GetValue();

                if (count <= maxCountToMerge)
                {
                    splitItems.Add(new(index, item));
                    actualInsertCount += count;

                    return true;
                }

                StateCloneHint hint;
                hint.isNested = false;
                var clonedItem = item.GetClone(hint);
                clonedItem.Count.Value = maxCountToMerge;
                splitItems.Add(new(index, clonedItem));

                item.Count.Value -= maxCountToMerge;
                leftCount -= maxCountToMerge;
                actualInsertCount += maxCountToMerge;

                return false;
            }
        }

        protected virtual void OnSplit(IContainerItem containerItem, int targetCount,
            ICollection<IContainerItem> splitItems, ref int actualSplitCount)
        {
            if (item.IsDestroyed || item.Count.GetValue() <= 0)
            {
                return;
            }

            var count = item.Count.GetValue().Min(targetCount);

            StateCloneHint hint;
            hint.isNested = false;
            var clonedItem = item.GetClone(hint);
            clonedItem.Count.Value = count;

            item.Count.Value -= count;

            splitItems.Add(clonedItem);

            actualSplitCount += count;
        }

        protected virtual void OnRemove(IContainerItem containerItem, int targetRemoveCount, ref int actualRemoveCount)
        {
            if (item.IsDestroyed || item.Count.GetValue() <= 0)
            {
                return;
            }

            var count = item.Count.GetValue().Min(targetRemoveCount);
            item.Count.Value -= count;
            actualRemoveCount += count;
        }
    }
}