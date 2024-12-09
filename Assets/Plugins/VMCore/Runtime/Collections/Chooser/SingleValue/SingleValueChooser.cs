using System;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public readonly partial struct SingleValueChooser<TItem> : IChooser<TItem>
    {
        public readonly TItem value;
        
        public SingleValueChooser(TItem value)
        {
            this.value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ResetChooser()
        {
            // Do nothing.
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TItem GetRandomItem(Random random)
        {
            return value;
        }

        object IRandomItemProvider.GetRandomObjectItem(Random random)
        {
            return value;
        }
    }
}