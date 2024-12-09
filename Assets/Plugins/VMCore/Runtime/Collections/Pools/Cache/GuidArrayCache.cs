using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Core.Pools
{
    public static class GuidArrayCache
    {
        private static readonly Stack<Guid[]> cache = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGet(out Guid[] array)
        {
            if (cache.Count > 0)
            {
                array = cache.Pop();
                return true;
            }

            array = null;
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid[] Get(int defaultSize)
        {
            if (cache.Count > 0)
            {
                return cache.Pop();
            }
            
            return new Guid[defaultSize];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Return(Guid[] array)
        {
            cache.Push(array);
        }
    }
}