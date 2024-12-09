using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public struct EllipseFloat : IKSet<Vector2>
    {
        public readonly Vector2 pivot;
        public readonly Vector2 radius;
        private Vector2? radiusSqr;

        public EllipseFloat(Vector2 pivot, Vector2 radius)
        {
            this.pivot = pivot;
            this.radius = radius;
            radiusSqr = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Vector2 pos)
        {
            Vector2 relativePos = pos - pivot;

            if (relativePos.AnyNumberAbove(radius))
            {
                return false;
            }

            radiusSqr ??= radius.Square();

            return relativePos.x * relativePos.x / radiusSqr.Value.x +
                relativePos.y * relativePos.y / radiusSqr.Value.y <= 1;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetIntegerPoints<TCollection>(TCollection collection)
            where TCollection : ICollection<Vector2Int>
        { 
            var min = (pivot - radius).FloorToZero();
            var max = (pivot + radius).FloorToZero();
            
            foreach (var chunkPos in new RectangleInteger(min, max))
            {
                if (Contains(chunkPos))
                {
                    collection.Add(chunkPos);
                }
            }
        }
    }
}