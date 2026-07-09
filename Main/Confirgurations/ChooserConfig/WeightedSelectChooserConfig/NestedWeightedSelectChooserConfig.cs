using System;
using System.Collections.Generic;
using VMFramework.Core;
using VMFramework.Core.Pools;
using Random = System.Random;

namespace VMFramework.Configuration
{
    [Serializable]
    public sealed class NestedWeightedSelectChooserConfig<TItem>
        : WeightedSelectChooserConfigBase<IChooser<TItem>, TItem>
    {
        private NestedWeightedSelectChooser<TItem> chooser;

        public NestedWeightedSelectChooserConfig()
        {
        }

        public NestedWeightedSelectChooserConfig(IEnumerable<IChooser<TItem>> items) : base(items)
        {
        }

        public override TItem GetRandomItem(Random random)
        {
            chooser ??= new NestedWeightedSelectChooser<TItem>(items);
            return chooser.GetRandomItem(random);
        }

        public override IChooser<TItem> GenerateNewChooser()
        {
            var newChoosers = ListPool<WeightedSelectItem<IChooser<TItem>>>.Default.Get();
            newChoosers.Clear();
            foreach (var item in items)
            {
                var chooser = item.value?.GenerateNewChooser();
                var ratio = item.ratio;
                var weightedItem = new WeightedSelectItem<IChooser<TItem>>(chooser, ratio);
                newChoosers.Add(weightedItem);
            }

            var generatedChooser = new NestedWeightedSelectChooser<TItem>(newChoosers);
            newChoosers.ReturnToDefaultPool();
            return generatedChooser;
        }

        protected override TItem UnboxWrapper(IChooser<TItem> wrapper)
        {
            return wrapper.GetRandomItem(GlobalRandom.Default);
        }
    }
}