using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EnumsNET;
using VMFramework.Core.Linq;
using VMFramework.Core.Pools;

namespace VMFramework.Core
{
    public static class CollectionRandomOperations
    {
        #region Choose

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Choose<T>(this Random random, IEnumerable<T> enumerable)
        {
            var array = enumerable.ToArrayDefaultPooled();
            var result = random.Choose(array);
            array.ReturnToDefaultPool();
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Choose<T>(this Random random, IReadOnlyCollection<T> collection)
        {
            var array = collection.ToArrayDefaultPooled();
            var result = random.Choose(array);
            array.ReturnToDefaultPool();
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Choose<T>(this Random random, IList<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (list.Count == 0)
            {
                throw new ArgumentException("List is empty.", nameof(list));
            }

            return list[random.Next(list.Count)];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Choose<T>(this Random random, T[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Length == 0)
            {
                throw new ArgumentException("Array is empty.", nameof(array));
            }

            return array[random.Next(array.Length)];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TKey ChooseKey<TKey, TValue>(this Random random, IDictionary<TKey, TValue> dictionary)
        {
            var keys = dictionary.Keys.ToArrayDefaultPooled(dictionary.Count);
            var result = random.Choose(keys);
            keys.ReturnToDefaultPool();
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue ChooseValue<TKey, TValue>(this Random random, IDictionary<TKey, TValue> dictionary)
        {
            var values = dictionary.Values.ToArrayDefaultPooled(dictionary.Count);
            var result = random.Choose(values);
            values.ReturnToDefaultPool();
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T WeightedChoose<T>(this Random random, IList<T> objects, IList<int> rates)
        {
            if (objects.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(objects));
            }

            if (objects.Count == 1 || rates.Count == 1)
            {
                return objects[0];
            }

            int length = rates.Count.ClampMax(objects.Count);

            int sum = rates.Sum(length);

            if (sum < 1)
            {
                return objects[0];
            }

            int randomRate = random.Range(1, sum);

            int cumulativeRate = 0;
            for (int i = 0; i < length; i++)
            {
                cumulativeRate += rates[i];
                if (cumulativeRate >= randomRate)
                {
                    return objects[i];
                }
            }

            throw new InvalidOperationException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T WeightedChoose<T>(this Random random, IList<T> objects, IList<float> rates)
        {
            if (objects.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(objects));
            }

            if (objects.Count == 1 || rates.Count == 1)
            {
                return objects[0];
            }

            int length = rates.Count.ClampMax(objects.Count);

            float sum = rates.Sum(length);

            if (sum <= 0)
            {
                return objects[0];
            }

            float randomRate = random.Range(sum);

            float cumulativeRate = 0;
            for (int i = 0; i < length; i++)
            {
                cumulativeRate += rates[i];
                if (cumulativeRate >= randomRate)
                {
                    return objects[i];
                }
            }

            throw new InvalidOperationException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TItem WeightedChoose<TItem>(this Random random, IList<(TItem item, float rate)> infos)
        {
            if (infos.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(infos));
            }

            if (infos.Count == 1)
            {
                return infos[0].item;
            }

            float sum = 0;

            foreach (var info in infos)
            {
                sum += info.rate;
            }

            if (sum <= 0)
            {
                return infos[0].item;
            }

            float randomRate = random.Range(sum);

            float cumulativeRate = 0;
            for (int i = 0; i < infos.Count; i++)
            {
                cumulativeRate += infos[i].rate;
                if (cumulativeRate >= randomRate)
                {
                    return infos[i].item;
                }
            }

            throw new InvalidOperationException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TEnum ChooseFlag<TEnum>(this Random random, TEnum enumValue)
            where TEnum : struct, Enum =>
            random.Choose(enumValue.GetFlags());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Choose<T>(this IEnumerable<T> enumerable) => GlobalRandom.Default.Choose(enumerable);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Choose<T>(this IReadOnlyCollection<T> collection) => GlobalRandom.Default.Choose(collection);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Choose<T>(this IList<T> list) => GlobalRandom.Default.Choose(list);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Choose<T>(this T[] array) => GlobalRandom.Default.Choose(array);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TKey ChooseKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) =>
            GlobalRandom.Default.ChooseKey(dictionary);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue ChooseValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) =>
            GlobalRandom.Default.ChooseValue(dictionary);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T WeightedChoose<T>(this IList<T> objects, IList<int> rates) =>
            GlobalRandom.Default.WeightedChoose(objects, rates);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T WeightedChoose<T>(this IList<T> objects, IList<float> rates) =>
            GlobalRandom.Default.WeightedChoose(objects, rates);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TItem WeightedChoose<TItem>(this IList<(TItem item, float rate)> infos) =>
            GlobalRandom.Default.WeightedChoose(infos);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TEnum ChooseFlag<TEnum>(this TEnum enumValue)
            where TEnum : struct, Enum =>
            GlobalRandom.Default.ChooseFlag(enumValue);

        #endregion

        #region Choose Or Default

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ChooseOrDefault<T>(this Random random, IEnumerable<T> enumerable, T defaultValue = default)
        {
            if (enumerable.IsNullOrEmpty())
            {
                return defaultValue;
            }
            
            return random.Choose(enumerable);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ChooseOrDefault<T>(this Random random, IReadOnlyCollection<T> collection, T defaultValue = default)
        {
            if (collection.IsNullOrEmpty())
            {
                return defaultValue;
            }
            
            return random.Choose(collection);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ChooseOrDefault<T>(this Random random, IList<T> list, T defaultValue = default)
        {
            if (list == null || list.Count == 0)
            {
                return defaultValue;
            }

            return list[random.Next(list.Count)];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ChooseOrDefault<T>(this IEnumerable<T> enumerable, T defaultValue = default)
        {
            return GlobalRandom.Default.ChooseOrDefault(enumerable, defaultValue);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ChooseOrDefault<T>(this IReadOnlyCollection<T> collection, T defaultValue = default)
        {
            return GlobalRandom.Default.ChooseOrDefault(collection, defaultValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ChooseOrDefault<T>(this IList<T> list, T defaultValue = default)
        {
            return GlobalRandom.Default.ChooseOrDefault(list, defaultValue);
        }

        #endregion
        
        #region Shuffle

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Shuffle<T>(this Random random, IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Range(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Shuffle<T>(this Random random, IList<T> list, int startIndex, int endIndex)
        {
            int n = endIndex + 1;
            while (n > startIndex + 1)
            {
                n--;
                int k = random.Range(startIndex, n);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Shuffle<T>(this IList<T> list) => GlobalRandom.Default.Shuffle(list);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Shuffle<T>(this IList<T> list, int startIndex, int endIndex) =>
            GlobalRandom.Default.Shuffle(list, startIndex, endIndex);

        #endregion
    }
}