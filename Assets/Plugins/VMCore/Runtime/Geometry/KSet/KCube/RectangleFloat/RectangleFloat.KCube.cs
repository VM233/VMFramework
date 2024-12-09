using System.Runtime.CompilerServices;
using UnityEngine;
using Random = System.Random;

namespace VMFramework.Core
{
    public partial struct RectangleFloat
    {
        Vector2 IMinMaxOwner<Vector2>.Min
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => min;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            init => min = value;
        }

        Vector2 IMinMaxOwner<Vector2>.Max
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => max;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            init => max = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Vector2 pos) => pos.x >= min.x && pos.x <= max.x &&
                                             pos.y >= min.y && pos.y <= max.y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 GetRelativePos(Vector2 pos) => pos - min;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 ClampMin(Vector2 pos) => pos.ClampMin(min);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 ClampMax(Vector2 pos) => pos.ClampMax(max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2 GetRandomItem(Random random) => random.Range(min, max);
        
        object IRandomItemProvider.GetRandomObjectItem(Random random)
        {
            return GetRandomItem(random);
        }

        void IChooser.ResetChooser()
        {
            
        }
    }
}