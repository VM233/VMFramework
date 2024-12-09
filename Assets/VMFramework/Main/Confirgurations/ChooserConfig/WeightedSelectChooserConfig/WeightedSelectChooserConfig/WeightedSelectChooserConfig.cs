using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public class WeightedSelectChooserConfig<TItem> : WeightedSelectChooserConfig<TItem, TItem>, 
        IWeightedSelectChooserConfig<TItem>
        where TItem : new()
    {
        public WeightedSelectChooserConfig() : base()
        {
            
        }
        
        public WeightedSelectChooserConfig(IEnumerable<TItem> items) : base(items)
        {
            
        }

        protected override TItem UnboxWrapper(TItem wrapper)
        {
            return wrapper;
        }
    }
    
    [TypeInfoBox("Choose a value from weighted items!")]
    public abstract partial class WeightedSelectChooserConfig<TWrapper, TItem>
        : WeightedSelectChooserConfigBase<TWrapper, TItem>, IWeightedSelectChooserConfig<TWrapper, TItem>
    {
        private WeightedSelectChooser<TItem> chooser;

        protected WeightedSelectChooserConfig() : base()
        {
            
        }

        protected WeightedSelectChooserConfig(IEnumerable<TWrapper> items) : base(items)
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