using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class RangeUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeInteger GetRange(this int start, int end)
        {
            return new RangeInteger(start, end);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RangeInteger GetRange(this int length)
        {
            return new RangeInteger(0, length - 1);
        }
    }
}