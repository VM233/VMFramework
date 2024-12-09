using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core.Pools;

namespace VMFramework.GameLogicArchitecture
{
    public static class GameTagCategorizeUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CategorizedByGameTags<TOwner, TEnumerable>(this TEnumerable enumerable,
            Dictionary<string, List<TOwner>> result)
            where TOwner : IGameTagsOwner
            where TEnumerable : IEnumerable<TOwner>
        {
            foreach (var owner in enumerable)
            {
                if (owner == null)
                {
                    continue;
                }

                foreach (var tag in owner.GameTags)
                {
                    if (result.TryGetValue(tag, out var owners) == false)
                    {
                        owners = ListPool<TOwner>.Default.Get();
                        owners.Clear();
                        result.Add(tag, owners);
                    }
                    
                    owners.Add(owner);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CategorizedByGameTags<TOwner, TTargetOwner>(this IEnumerable<TOwner> owners,
            Dictionary<string, List<TTargetOwner>> result)
            where TOwner : IGameTagsOwner
            where TTargetOwner : IGameTagsOwner
        {
            foreach (var owner in owners)
            {
                if (owner is not TTargetOwner targetOwner)
                {
                    continue;
                }
                
                foreach (var tag in targetOwner.GameTags)
                {
                    if (result.TryGetValue(tag, out var targetOwners) == false)
                    {
                        targetOwners = ListPool<TTargetOwner>.Default.Get();
                        targetOwners.Clear();
                        result.Add(tag, targetOwners);
                    }
                    
                    targetOwners.Add(targetOwner);
                }
            }
        }
    }
}