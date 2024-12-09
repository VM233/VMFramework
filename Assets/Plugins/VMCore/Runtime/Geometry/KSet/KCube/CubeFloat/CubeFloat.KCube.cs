using System.Runtime.CompilerServices;
using UnityEngine;
using Random = System.Random;

namespace VMFramework.Core
{
    public partial struct CubeFloat
    {
        Vector3 IMinMaxOwner<Vector3>.Min
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => min;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            init => min = value;
        }

        Vector3 IMinMaxOwner<Vector3>.Max
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => max;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            init => max = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Vector3 pos) => pos.x >= min.x && pos.x <= max.x &&
                                             pos.y >= min.y && pos.y <= max.y &&
                                             pos.z >= min.z && pos.z <= max.z;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 GetRelativePos(Vector3 pos) => pos - min;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 ClampMin(Vector3 pos) => pos.ClampMin(min);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 ClampMax(Vector3 pos) => pos.ClampMax(max);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 GetRandomItem(Random random) => random.Range(min, max);
        
        object IRandomItemProvider.GetRandomObjectItem(Random random)
        {
            return GetRandomItem(random);
        }

        void IChooser.ResetChooser()
        {
            
        }
    }
}