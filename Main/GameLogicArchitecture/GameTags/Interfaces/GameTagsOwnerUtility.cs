using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.GameLogicArchitecture
{
    public static class GameTagsOwnerUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TOwner> SelectWithGameTag<TOwner>(this IEnumerable<TOwner> owners, string gameTag)
            where TOwner : IGameTagsOwner
        {
            foreach (var owner in owners)
            {
                if (owner == null)
                {
                    continue;
                }

                if (owner.GameTags.Contains(gameTag))
                {
                    yield return owner;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TOwner> SelectWithAnyGameTags<TOwner, TCollection>(this IEnumerable<TOwner> owners,
            TCollection gameTags)
            where TOwner : IGameTagsOwner
            where TCollection : IEnumerable<string>
        {
            foreach (var owner in owners)
            {
                if (owner == null)
                {
                    continue;
                }

                foreach (var gameTag in gameTags)
                {
                    if (owner.GameTags.Contains(gameTag))
                    {
                        yield return owner;
                        break;
                    }
                }
            }
        }
    }
}