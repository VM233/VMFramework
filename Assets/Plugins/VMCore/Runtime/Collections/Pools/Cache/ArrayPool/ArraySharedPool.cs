using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace VMFramework.Core.Pools
{
    /// <summary>
    /// Thread-safe pool of arrays of type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ArraySharedPool<T>
    {
        private const int LENGTH_STEP = 5;
        private static readonly ConcurrentDictionary<int, DefaultConcurrentPool<T[]>> pools = new();
        private static readonly Func<int, DefaultConcurrentPool<T[]>> createPoolFunc = CreatePool;
        
        private static DefaultConcurrentPool<T[]> CreatePool(int length)
        {
            return new(new ArrayPoolPolicy<T>(length), 8);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] Get(int length)
        {
            var pool = pools.GetOrAdd(length, createPoolFunc);

            return pool.Get();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] GetByMinLength(int minLength)
        {
            var leftLength = minLength % LENGTH_STEP;
            int length;
            if (leftLength == 0)
            {
                length = minLength;
            }
            else
            {
                length = minLength + LENGTH_STEP - leftLength;
            }
            
            return Get(length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Return(T[] array)
        {
            var pool = pools.GetOrAdd(array.Length, createPoolFunc);

            pool.Return(array);
        }
    }
}