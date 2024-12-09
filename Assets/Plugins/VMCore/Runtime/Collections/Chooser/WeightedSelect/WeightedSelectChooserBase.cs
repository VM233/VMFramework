using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public abstract class WeightedSelectChooserBase<TItem>
    {
        protected readonly (TItem item, float weight)[] infos;

        protected WeightedSelectChooserBase(params (TItem item, float weight)[] infos)
        {
            this.infos = infos;
            Check();
        }

        protected WeightedSelectChooserBase(TItem[] values, float[] weights)
        {
            if (values.Length == 0)
            {
                Debug.LogError($"{nameof(values)} array is empty.");
                return;
            }
            
            if (weights.Length == 0)
            {
                Debug.LogError($"{nameof(weights)} array is empty.");
                return;
            }

            if (values.Length != weights.Length)
            {
                Debug.LogError($"Length of {nameof(values)} and {nameof(weights)} arrays do not match.");
                return;
            }

            infos = new (TItem, float)[values.Length];
            for (int i = 0; i < weights.Length; i++)
            {
                infos[i] = (values[i], weights[i]);
            }
            
            Check();
        }

        protected WeightedSelectChooserBase(IReadOnlyList<TItem> values)
        {
            infos = new (TItem, float)[values.Count];
            for (int i = 0; i < values.Count; i++)
            {
                infos[i] = (values[i], 1f);
            }
            Check();
        }

        protected WeightedSelectChooserBase(IReadOnlyList<WeightedSelectItem<TItem>> items)
        {
            infos = new (TItem, float)[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                infos[i] = (items[i].value, items[i].weight);
            }
            
            Check();
        }

        protected WeightedSelectChooserBase(IReadOnlyList<IWeightedSelectItem<TItem>> items)
        {
            infos = new (TItem, float)[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                infos[i] = (items[i].Value, items[i].Weight);
            }
            
            Check();
        }

        protected WeightedSelectChooserBase(IReadOnlyDictionary<TItem, float> itemDict)
        {
            infos = new (TItem, float)[itemDict.Count];
            int i = 0;
            foreach (var kvp in itemDict)
            {
                infos[i] = (kvp.Key, kvp.Value);
                i++;
            }
            
            Check();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Check()
        {
            for (int i = 0; i < infos.Length; i++)
            {
                if (infos[i].weight <= 0f)
                {
                    Debug.LogError($"Weight of item: (index = {i}, item = {infos[i].item}) " +
                                   $"is less than or equal to 0.");
                }
            }
        }
    }
}