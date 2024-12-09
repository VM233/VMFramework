using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class ColorCreationUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color CreateColorFrom255(this float value) => new(value / 255f, value / 255f, value / 255f);
    }
}