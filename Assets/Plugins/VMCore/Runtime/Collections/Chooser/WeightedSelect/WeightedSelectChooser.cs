using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Random = System.Random;

namespace VMFramework.Core
{
    public sealed partial class WeightedSelectChooser<TItem> : WeightedSelectChooserBase<TItem>, IChooser<TItem>
    {
        public WeightedSelectChooser(params (TItem item, float weight)[] infos) : base(infos)
        {
        }

        public WeightedSelectChooser(TItem[] values, float[] weights) : base(values, weights)
        {
        }

        public WeightedSelectChooser(IReadOnlyList<TItem> values) : base(values)
        {
        }

        public WeightedSelectChooser(IReadOnlyList<WeightedSelectItem<TItem>> items) : base(items)
        {
        }

        public WeightedSelectChooser(IReadOnlyList<IWeightedSelectItem<TItem>> items) : base(items)
        {
        }

        public WeightedSelectChooser(IReadOnlyDictionary<TItem, float> itemDict) : base(itemDict)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TItem GetRandomItem(Random random)
        {
            return random.WeightedChoose(infos);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResetChooser()
        {
            // No need to reset anything for this chooser.
        }
    }
}