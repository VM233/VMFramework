using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core.Pools;

namespace VMFramework.Core
{
    public static class IDOwnerUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TOwner> SelectWithID<TOwner, TID>(this IEnumerable<TOwner> owners, TID id)
            where TOwner : IIDOwner<TID>
            where TID : IEquatable<TID>
        {
            foreach (var owner in owners)
            {
                if (owner == null)
                {
                    continue;
                }

                if (owner.id.Equals(id))
                {
                    yield return owner;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TOwner> SelectWithAnyIDs<TOwner, TID>(this IEnumerable<TOwner> owners,
            IEnumerable<TID> ids)
            where TOwner : IIDOwner<TID>
            where TID : IEquatable<TID>
        {
            foreach (var owner in owners)
            {
                if (owner == null)
                {
                    continue;
                }

                foreach (var id in ids)
                {
                    if (owner.id.Equals(id))
                    {
                        yield return owner;
                        break;
                    }
                }
            }
        }

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
        public static void BuildIDDictionary<TOwner, TDictionary>(this IEnumerable<TOwner> owners,
            TDictionary idDictionary, Func<List<TOwner>> listFactory = null)
            where TOwner : IIDOwner<string>
            where TDictionary : IDictionary<string, List<TOwner>>
        {
            foreach (var owner in owners)
            {
                if (owner == null)
                {
                    continue;
                }

                List<TOwner> list;
                if (listFactory == null)
                {
                    list = idDictionary.GetOrCreate(owner.id);
                }
                else
                {
                    list = idDictionary.GetOrCreateFromFactory(owner.id, listFactory);
                }
                list.Add(owner);
            }
        }
    }
}