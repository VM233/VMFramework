using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.GameLogicArchitecture
{
    public static class GameTagQueryUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasTag<TOwner>(this TOwner owner, string tag)
            where TOwner : IGameTagsOwner
        {
            return owner.GameTags.Contains(tag);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAnyTag<TOwner, TEnumerable>(this TOwner owner, TEnumerable tags)
            where TOwner : IGameTagsOwner
            where TEnumerable : IEnumerable<string>
        {
            foreach (var tag in tags)
            {
                if (owner.GameTags.Contains(tag))
                {
                    return true;
                }
            }
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAllTags<TOwner, TEnumerable>(this TOwner owner, TEnumerable tags)
            where TOwner : IGameTagsOwner
            where TEnumerable : IEnumerable<string>
        {
            foreach (var tag in tags)
            {
                if (owner.GameTags.Contains(tag) == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}