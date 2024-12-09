using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public partial struct UniformlySpacedRangeFloat
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniformlySpacedRangeFloat ExcludeBoundaries(float min, float max, int count)
        {
            if (count < 2)
            {
                return new(min, max, count);
            }
            
            float step = (max - min) / (count + 1);
            return new(min + step, max - step, count);
        }
    }
}