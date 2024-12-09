using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace VMFramework.Core.Pools
{
    public static class DictionaryPoolUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReturnToSharedPool<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) =>
            DictionaryPool<TKey, TValue>.Shared.Return(dictionary);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReturnToDefaultPool<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) =>
            DictionaryPool<TKey, TValue>.Default.Return(dictionary);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReturnToDefaultPoolRecursively<TKey, TItem>(this Dictionary<TKey, List<TItem>> dictionary)
        {
            foreach (var list in dictionary.Values)
            {
                ListPool<TItem>.Default.Return(list);
            }

            dictionary.Clear();
            DictionaryPool<TKey, List<TItem>>.Default.Return(dictionary);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<TItem> GetOrAddFromDefaultPool<TKey, TItem>(this Dictionary<TKey, List<TItem>> dictionary,
            [NotNull] TKey key)
        {
            if (dictionary.TryGetValue(key, out var list))
            {
                return list;
            }

            list = ListPool<TItem>.Default.Get();
            list.Clear();
            dictionary[key] = list;
            return list;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HashSet<TItem> GetOrAddFromDefaultPool<TKey, TItem>(
            this Dictionary<TKey, HashSet<TItem>> dictionary, [NotNull] TKey key)
        {
            if (dictionary.TryGetValue(key, out var collection))
            {
                return collection;
            }

            collection = HashSetPool<TItem>.Default.Get();
            collection.Clear();
            dictionary[key] = collection;
            return collection;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RemoveAndReturnToDefaultPool<TKey, TItem, TCollection>(
            this Dictionary<TKey, TCollection> dictionary, [NotNull] TKey key, [NotNull] TItem item)
            where TCollection : class, ICollection<TItem>, new()
        {
            if (dictionary.TryGetValue(key, out var collection) == false)
            {
                return false;
            }

            if (collection.Remove(item) == false)
            {
                return false;
            }

            if (collection.Count == 0)
            {
                dictionary.Remove(key);
                CollectionPool<TCollection>.Default.Return(collection);
            }

            return true;
        }
    }
}