using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Containers
{
    public class GeneralContainerRange : MonoBehaviour
    {
        [MinValue(0)]
        public int minIndex = 0;

        [MinValue(nameof(minIndex))]
        public int maxIndex = int.MaxValue;

        public bool enableQuery = false;

        [ShowIf(nameof(enableQuery))]
        public bool enableQueryNullIntention = true;

        [CommonPreset(ContainerGeneralSetting.CONTAINER_INTENTION_PRESET_KEY)]
        [ShowIf(nameof(enableQuery))]
        public List<string> limitQueryIntentions = new();

        public bool addable = true;

        [ShowIf(nameof(addable))]
        public bool enableAddNullIntention = true;

        [CommonPreset(ContainerGeneralSetting.CONTAINER_INTENTION_PRESET_KEY)]
        [ShowIf(nameof(addable))]
        public List<string> limitAddIntentions = new();

        [CommonPreset(PriorityDefinesPreset.NAME)]
        [ShowIf(nameof(addable))]
        public int addPriority = 0;

        public bool sortable = true;

        public bool enableFilter = false;

        [ShowIf(nameof(enableFilter))]
        public bool enableFilterNullIntention = true;

        [CommonPreset(ContainerGeneralSetting.CONTAINER_INTENTION_PRESET_KEY)]
        [ShowIf(nameof(enableFilter))]
        public List<string> limitFilterIntentions = new();

        [GameTagID]
        [ShowIf(nameof(enableFilter))]
        public List<string> limitTags = new();

        public bool enableGlobalFilter = false;

        [ShowIf(nameof(enableGlobalFilter))]
        public bool enableGlobalFilterNullIntention = true;

        [CommonPreset(ContainerGeneralSetting.CONTAINER_INTENTION_PRESET_KEY)]
        [ShowIf(nameof(enableGlobalFilter))]
        public List<string> limitGlobalFilterIntentions = new();

        [GameTagID]
        [ShowIf(nameof(enableGlobalFilter))]
        public List<string> limitGlobalTags = new();

        protected IContainer container;

        protected virtual void Awake()
        {
            container = GetComponentInParent<IContainer>();

            if (enableQuery)
            {
                container.OnQueryItems += OnQueryItems;
            }

            if (addable)
            {
                container.OnCollectAddPriorityRanges += OnCollectAddPriorityRanges;
            }

            if (sortable)
            {
                container.OnCollectSortableRanges += OnCollectSortableRanges;
            }

            if (enableFilter || enableGlobalFilter)
            {
                container.OnFilterMatch += OnFilterMatch;
            }
        }

        protected virtual void OnQueryItems(IContainer container, ContainerQueryArguments arguments,
            ICollection<IContainerItem> items)
        {
            if (arguments.intention == null)
            {
                if (enableQueryNullIntention == false)
                {
                    return;
                }
            }
            else
            {
                if (limitQueryIntentions.IsNullOrEmpty() == false)
                {
                    if (limitQueryIntentions.Contains(arguments.intention) == false)
                    {
                        return;
                    }
                }
            }

            var range = new RangeInteger(minIndex, maxIndex);
            range = container.ClampSlotRange(range);

            if (arguments.range is { } limitRange)
            {
                range.min = range.min.Max(limitRange.min);
                range.max = range.max.Min(limitRange.max);
            }

            if (range.min > range.max)
            {
                return;
            }

            foreach (var index in range)
            {
                var item = container.GetItem(index);
                if (item == null)
                {
                    continue;
                }

                items.Add(item);
            }
        }

        protected virtual void OnCollectAddPriorityRanges(IContainer container, IContainerItem item, string intention,
            ICollection<(RangeInteger range, int priority)> ranges)
        {
            if (intention == null)
            {
                if (enableAddNullIntention == false)
                {
                    return;
                }
            }
            else
            {
                if (limitAddIntentions.IsNullOrEmpty() == false)
                {
                    if (limitAddIntentions.Contains(intention) == false)
                    {
                        return;
                    }
                }
            }

            ranges.Add((new(minIndex, maxIndex), addPriority));
        }

        protected virtual void OnCollectSortableRanges(IContainer container, ICollection<RangeInteger> ranges)
        {
            ranges.Add(new(minIndex, maxIndex));
        }

        protected virtual void OnFilterMatch(IContainer container, int? index, IContainerItem item, string intention,
            ref bool isMatch)
        {
            if (enableGlobalFilter)
            {
                if (intention == null)
                {
                    if (enableGlobalFilterNullIntention == false)
                    {
                        goto NEXT_FILTER;
                    }
                }
                else
                {
                    if (limitGlobalFilterIntentions.IsNullOrEmpty() == false)
                    {
                        if (limitGlobalFilterIntentions.Contains(intention) == false)
                        {
                            goto NEXT_FILTER;
                        }
                    }
                }

                if (limitGlobalTags.IsNullOrEmpty() == false)
                {
                    if (item.HasAnyTags(limitGlobalTags) == false)
                    {
                        isMatch = false;
                        return;
                    }
                }
            }
            
            NEXT_FILTER:

            if (enableFilter == false)
            {
                return;
            }

            if (index == null)
            {
                return;
            }

            if (index < minIndex || index > maxIndex)
            {
                return;
            }

            if (intention == null)
            {
                if (enableFilterNullIntention == false)
                {
                    return;
                }
            }
            else
            {
                if (limitFilterIntentions.IsNullOrEmpty() == false)
                {
                    if (limitFilterIntentions.Contains(intention) == false)
                    {
                        return;
                    }
                }
            }

            if (limitTags.IsNullOrEmpty() == false)
            {
                if (item.HasAnyTags(limitTags) == false)
                {
                    isMatch = false;
                }
            }
        }
    }
}