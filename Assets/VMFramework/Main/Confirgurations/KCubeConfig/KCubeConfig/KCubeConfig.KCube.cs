using System;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public partial class KCubeConfig<TPoint>
    {
        TPoint IKCubeConfig<TPoint>.Min
        {
            get => min;
            set => min = value;
        }

        TPoint IKCubeConfig<TPoint>.Max
        {
            get => max;
            set => max = value;
        }

        TPoint IMinMaxOwner<TPoint>.Min
        {
            get => min;
            init => min = value;
        }

        TPoint IMinMaxOwner<TPoint>.Max
        {
            get => max;
            init => max = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract TPoint ClampMin(TPoint pos);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract TPoint ClampMax(TPoint pos);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract TPoint GetRelativePos(TPoint pos);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract bool Contains(TPoint pos);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public abstract TPoint GetRandomItem(Random random);
    }
}