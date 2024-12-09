using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Core.Pools
{
    /// <summary>
    /// A simple pool for classes with a parameterless constructor, but it's not thread-safe.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class SimplePool<T> where T : class, new()
    {
        private static readonly Queue<T> pool = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Get()
        {
            if (pool.Count == 0)
            {
                return new T();
            }
            
            return pool.Dequeue();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Return(T obj)
        {
            pool.Enqueue(obj);
        }
    }
}