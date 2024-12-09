using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core.Pools;

namespace VMFramework.Core
{
    public static class IDOwnerUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BuildUniqueIDDictionary<TOwner>(this IEnumerable<TOwner> owners,
            out Dictionary<string, TOwner> idDictionary)
            where TOwner : IIDOwner<string>
        {
            idDictionary = new();

            foreach (var owner in owners)
            {
                if (owner == null)
                {
                    continue;
                }

                idDictionary[owner.id] = owner;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BuildIDDictionary<TOwner>(this IEnumerable<TOwner> owners,
            out Dictionary<string, List<TOwner>> idDictionary)
            where TOwner : IIDOwner<string>
        {
            idDictionary = DictionaryPool<string, List<TOwner>>.Default.Get();
            idDictionary.Clear();

            foreach (var owner in owners)
            {
                if (owner == null)
                {
                    continue;
                }

                var list = idDictionary.GetOrAddFromDefaultPool(owner.id);
                list.Add(owner);
            }
        }
    }
}