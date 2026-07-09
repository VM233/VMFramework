using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Properties
{
    public static class CollectionPropertyUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InitializeValues<TProperty, TValue>(this TProperty collectionProperty,
            IEnumerable<TValue> values)
            where TProperty : ICollectionProperty<TValue>
        {
            collectionProperty.Clear();
            collectionProperty.AddRange(values, initial: true);
        }
    }
}