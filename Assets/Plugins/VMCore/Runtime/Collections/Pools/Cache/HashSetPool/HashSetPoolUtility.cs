using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Core.Pools
{
    public static class HashSetPoolUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HashSet<T> ToHashSetSharedPooled<T>(this IEnumerable<T> enumerable)
        {
            var hashSet = HashSetPool<T>.Shared.Get();
            hashSet.Clear();
            hashSet.UnionWith(enumerable);
            return hashSet;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HashSet<T> ToHashSetDefaultPooled<T>(this IEnumerable<T> enumerable)
        {
            var hashSet = HashSetPool<T>.Default.Get();
            hashSet.Clear();
            hashSet.UnionWith(enumerable);
            return hashSet;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReturnToSharedPool<T>(this HashSet<T> hashSet) => HashSetPool<T>.Shared.Return(hashSet);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReturnToDefaultPool<T>(this HashSet<T> hashSet) =>
            HashSetPool<T>.Default.Return(hashSet);
    }
}