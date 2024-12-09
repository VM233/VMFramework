using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Core.Pools
{
    public static class ListPoolUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> ToListSharedPooled<T>(this IEnumerable<T> enumerable)
        {
            var list = ListPool<T>.Shared.Get();
            list.Clear();
            list.AddRange(enumerable);
            return list;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> ToListDefaultPooled<T>(this IEnumerable<T> enumerable)
        {
            var list = ListPool<T>.Default.Get();
            list.Clear();
            list.AddRange(enumerable);
            return list;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReturnToSharedPool<T>(this List<T> list) => ListPool<T>.Shared.Return(list);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReturnToDefaultPool<T>(this List<T> list) => ListPool<T>.Default.Return(list);
    }
}