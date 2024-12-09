using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class KSetAssert
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AssertContainsBy<TKSet>(this Vector3Int position, TKSet set, string positionName,
            string setName = null)
            where TKSet : IKCube<Vector3Int>
        {
            if (set.Contains(position))
            {
                return;
            }

            setName = setName == null ? set.ToString() : $"{setName}: {set.ToString()}";
            throw new ArgumentException($"{positionName}:{position} does not contain by {setName}");
        }
    }
}