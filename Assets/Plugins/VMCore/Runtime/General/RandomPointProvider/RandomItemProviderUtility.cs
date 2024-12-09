using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class RandomItemProviderUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TItem GetRandomItem<TItem>(this IRandomItemProvider<TItem> randomItemProvider)
        {
            return randomItemProvider.GetRandomItem(GlobalRandom.Default);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TItem> GetRandomItems<TItem>(this IRandomItemProvider<TItem> randomItemProvider,
            int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return randomItemProvider.GetRandomItem();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetRandomItems<TItem, TRandomItemProvider>(this TRandomItemProvider randomItemProvider,
            int count, ICollection<TItem> items, Random random)
            where TRandomItemProvider : IRandomItemProvider<TItem>
        {
            for (int i = 0; i < count; i++)
            {
                items.Add(randomItemProvider.GetRandomItem(random));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetRandomItems<TItem, TRandomItemProvider>(this TRandomItemProvider randomItemProvider,
            int count, ref TItem[] items, Random random) where TRandomItemProvider : IRandomItemProvider<TItem>
        {
            count.CreateOrResizeArrayWithMinLength(ref items);
            
            for (int i = 0; i < count; i++)
            {
                items[i] = randomItemProvider.GetRandomItem(random);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetRandomItems<TItem, TRandomItemProvider>(this TRandomItemProvider randomItemProvider,
            int count, ref TItem[] items)
            where TRandomItemProvider : IRandomItemProvider<TItem>
        {
            randomItemProvider.GetRandomItems(count, ref items, GlobalRandom.Default);
        }
    }
}