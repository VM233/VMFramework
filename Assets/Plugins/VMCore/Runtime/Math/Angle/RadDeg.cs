using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class RadDeg
    {
        /// <summary>
        /// Converts an angle in radians to degrees.
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RadToDeg(this float rad)
        {
            return rad * Constants.Rad2Deg;
        }
        
        /// <summary>
        /// Converts an angle in degrees to radians.
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DegToRad(this float deg)
        {
            return deg * Constants.Deg2Rad;
        }
    }
}