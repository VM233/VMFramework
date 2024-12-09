using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public struct EllipsoidFloat : IKSet<Vector3>
    {
        public readonly Vector3 pivot;
        public readonly Vector3 radius;
        private Vector3? radiusSqr;

        public EllipsoidFloat(Vector3 pivot, Vector3 radius)
        {
            this.pivot = pivot;
            this.radius = radius;
            radiusSqr = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Vector3 pos)
        {
            Vector3 relative = pos - pivot;

            if (relative.AnyNumberAbove(radius))
            {
                return false;
            }
            
            radiusSqr ??= radius.Square();

            return relative.x * relative.x / radiusSqr.Value.x + relative.y * relative.y / radiusSqr.Value.y +
                relative.z * relative.z / radiusSqr.Value.z <= 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetIntegerPoints<TCollection>(TCollection collection)
            where TCollection : ICollection<Vector3Int>
        { 
            var min = (pivot - radius).FloorToZero();
            var max = (pivot + radius).FloorToZero();
            
            foreach (var chunkPos in new CubeInteger(min, max))
            {
                if (Contains(chunkPos))
                {
                    collection.Add(chunkPos);
                }
            }
        }
    }
}