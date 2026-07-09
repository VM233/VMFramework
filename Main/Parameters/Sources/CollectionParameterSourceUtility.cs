using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using VMFramework.Core.Pools;

namespace VMFramework.Parameters
{
    public static class CollectionParameterSourceUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetListFromDefaultPool<TValue>(
            [DisallowNull] this ICollectionParametersSource<TValue> source, out List<TValue> list)
        {
            list = ListPool<TValue>.Default.Get();
            list.Clear();

            if (source.TryGetValues(list))
            {
                return true;
            }

            list.ReturnToDefaultPool();
            list = null;
            return false;
        }
    }
}