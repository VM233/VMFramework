using System;
using System.Collections.Generic;

namespace VMFramework.Core
{
    public sealed class NestedWeightedSelectChooser<TItem> : WeightedSelectChooserBase<IChooser<TItem>>, IChooser<TItem>
    {
        public NestedWeightedSelectChooser(params (IChooser<TItem> item, float weight)[] infos) : base(infos)
        {
        }

        public NestedWeightedSelectChooser(IChooser<TItem>[] values, float[] weights) : base(values, weights)
        {
        }

        public NestedWeightedSelectChooser(IReadOnlyList<IChooser<TItem>> values) : base(values)
        {
        }

        public NestedWeightedSelectChooser(IReadOnlyList<WeightedSelectItem<IChooser<TItem>>> items) : base(items)
        {
        }

        public NestedWeightedSelectChooser(IReadOnlyList<IWeightedSelectItem<IChooser<TItem>>> items) : base(items)
        {
        }

        public NestedWeightedSelectChooser(IReadOnlyDictionary<IChooser<TItem>, float> itemDict) : base(itemDict)
        {
        }

        public void ResetChooser()
        {
            foreach (var (chooser, _) in infos)
            {
                chooser.ResetChooser();
            }
        }

        public TItem GetRandomItem(Random random)
        {
            var chooser = random.WeightedChoose(infos);
            return chooser.GetRandomItem(random);
        }
    }
}