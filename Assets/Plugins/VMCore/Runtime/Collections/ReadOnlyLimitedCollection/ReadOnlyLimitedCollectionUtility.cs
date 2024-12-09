using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class ReadOnlyLimitedCollectionUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFull<TItem>(this IReadOnlyLimitedCollection<TItem> collection)
        {
            return collection.Count >= collection.Capacity;
        }
    }
}