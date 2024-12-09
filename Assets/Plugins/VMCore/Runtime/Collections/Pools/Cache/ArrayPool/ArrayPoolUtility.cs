using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace VMFramework.Core.Pools
{
    public static class ArrayPoolUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ToLargerArrayDefaultPooled<T>(this IEnumerable<T> source)
        {
            var array = ArrayDefaultPool<T>.GetByMinLength(source.Count());

            var i = 0;

            foreach (var item in source)
            {
                array[i++] = item;
            }

            return array;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ToLargerArrayDefaultPooled<T>(this IEnumerable<T> source, int count)
        {
            var array = ArrayDefaultPool<T>.GetByMinLength(count);

            var i = 0;

            foreach (var item in source)
            {
                array[i++] = item;
            }

            return array;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ToArrayDefaultPooled<T>(this IEnumerable<T> source)
        {
            var array = ArrayDefaultPool<T>.Get(source.Count());

            var i = 0;

            foreach (var item in source)
            {
                array[i++] = item;
            }

            return array;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ToArrayDefaultPooled<T>(this IEnumerable<T> source, int count)
        {
            var array = ArrayDefaultPool<T>.Get(source.Count());

            var i = 0;

            foreach (var item in source)
            {
                array[i++] = item;
            }

            return array;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ToArrayDefaultPooled<T>(this IEnumerable<T> source, int start, int length)
        {
            var array = ArrayDefaultPool<T>.Get(length);

            var i = 0;
            
            foreach (var item in source.Skip(start))
            {
                if (i >= length) break;
                
                array[i++] = item;
            }
            
            return array;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ToArrayDefaultPooled<T>(this IReadOnlyCollection<T> source)
        {
            var array = ArrayDefaultPool<T>.Get(source.Count);

            var i = 0;

            foreach (var item in source)
            {
                array[i++] = item;
            }

            return array;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ToArrayDefaultPooled<T>(this IReadOnlyList<T> source, int start, int length)
        {
            var array = ArrayDefaultPool<T>.Get(length);

            for (var i = 0; i < length; i++)
            {
                array[i] = source[start + i];
            }
            
            return array;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReturnToDefaultPool<T>(this T[] array) => ArrayDefaultPool<T>.Return(array);
    }
}