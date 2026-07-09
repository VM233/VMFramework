using System.Runtime.CompilerServices;

namespace VMFramework.Properties
{
    public static class DictionaryPropertyUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Remove<TKey, TValue>(this IDictionaryProperty<TKey, TValue> dictionaryProperty, TKey key,
            bool initial)
        {
            return dictionaryProperty.Remove(key, initial, out _);
        }
    }
}