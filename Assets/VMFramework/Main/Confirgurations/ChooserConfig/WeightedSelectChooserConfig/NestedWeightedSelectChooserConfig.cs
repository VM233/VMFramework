using System.Collections.Generic;
using VMFramework.Core;
using Random = System.Random;

namespace VMFramework.Configuration
{
    public sealed class NestedWeightedSelectChooserConfig<TItem>
        : WeightedSelectChooserConfigBase<IChooserConfig<TItem>, TItem>, INestedChooserConfig<TItem>
    {
        private NestedWeightedSelectChooser<TItem> chooser;

        public NestedWeightedSelectChooserConfig()
        {
        }

        public NestedWeightedSelectChooserConfig(IEnumerable<IChooserConfig<TItem>> items) : base(items)
        {
        }

        public override TItem GetRandomItem(Random random)
        {
            chooser ??= new NestedWeightedSelectChooser<TItem>(items);
            return chooser.GetRandomItem(random);
        }

        public override IChooser<TItem> GenerateNewChooser()
        {
            return new NestedWeightedSelectChooser<TItem>(items);
        }

        protected override TItem UnboxWrapper(IChooserConfig<TItem> wrapper)
        {
            return wrapper.GetRandomItem(GlobalRandom.Default);
        }
    }
}