using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    [Serializable]
    public abstract partial class GeneralWeightedSelectChooserConfig<TWrapper, TItem>
        : WeightedSelectChooserConfigBase<TWrapper, TItem>, IWeightedSelectChooserConfig<TWrapper, TItem>
    {
        private WeightedSelectChooser<TItem> chooser;

        protected GeneralWeightedSelectChooserConfig() : base()
        {
            
        }

        protected GeneralWeightedSelectChooserConfig(IEnumerable<TWrapper> items) : base(items)
        {
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private WeightedSelectChooser<TItem> GenerateThisChooser()
        {
            return new WeightedSelectChooser<TItem>(items
                .Select(item => (UnboxWrapper(item.value), item.ratio.F())).ToArray());
        }

        public override IChooser<TItem> GenerateNewChooser()
        {
            return this;
        }

        public override TItem GetRandomItem(Random random)
        {
            chooser ??= GenerateThisChooser();
            
            return chooser.GetRandomItem(random);
        }
    }
}