using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    [Serializable]
    public abstract partial class WeightedSelectChooserConfigBase<TWrapper, TItem>
        : GeneralWrapperChooser<TWrapper, TItem>, IWeightedSelectChooserConfig<TWrapper, TItem>
    {
#if UNITY_EDITOR
        [OnValueChanged(nameof(OnItemsChangedGUI), true)]
        [OnCollectionChanged(nameof(OnItemsChangedGUI))]
        [ListDrawerSettings(CustomAddFunction = nameof(AddWeightedSelectItemGUI), NumberOfItemsPerPage = 6)]
#endif
        [IsNotNullOrEmpty]
        [JsonProperty]
        public List<WeightedSelectItemConfig<TWrapper>> items = new();
        
        protected WeightedSelectChooserConfigBase()
        {

        }

        protected WeightedSelectChooserConfigBase(IEnumerable<TWrapper> items)
        {
            this.items = items.Select(item => new WeightedSelectItemConfig<TWrapper>
            {
                value = item,
                ratio = 1
            }).ToList();
        }

        public override IEnumerable<TWrapper> GetAvailableWrappers()
        {
            return items.Select(item => item.value);
        }

        public override void SetAvailableValues(Func<TWrapper, TWrapper> setter)
        {
            foreach (var item in items)
            {
                item.value = setter(item.value);
            }
        }

        public bool ContainsWrapper(TWrapper wrapper)
        {
            return items.Any(item => item.value.Equals(wrapper));
        }

        public void AddWrapper(TWrapper wrapper)
        {
            items.Add(new WeightedSelectItemConfig<TWrapper>
            {
                value = wrapper,
                ratio = 1
            });

#if UNITY_EDITOR
            OnItemsChangedGUI();
#endif
        }

        public void RemoveWrapper(TWrapper wrapper)
        {
            items.RemoveAll(item => item.value.Equals(wrapper));
#if UNITY_EDITOR
            OnItemsChangedGUI();
#endif
        }

        public override string ToString()
        {
            if (items.Count == 0)
            {
                return "";
            }

            if (items.Count == 1)
            {
                return $"{WrapperToString(items[0].value)}";
            }

            var displayProbabilities = items.Select(item => item.ratio).UniqueCount() != 1;

            return ", ".Join(items.Select(item =>
            {
                var itemValueString = WrapperToString(item.value);

                if (displayProbabilities)
                {
                    itemValueString += $":{item.probability.ToString(1)}%";
                }

                return itemValueString;
            }));
        }
    }
}