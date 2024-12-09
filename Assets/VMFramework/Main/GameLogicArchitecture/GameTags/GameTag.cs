using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public static class GameTag
    {
        private static readonly Dictionary<string, GameTagInfo> tags = new();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddTag(GameTagInfo tagInfo)
        {
            if (tagInfo.id.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(tagInfo.id));
            }

            if (tags.TryAdd(tagInfo.id, tagInfo) == false)
            {
                throw new ArgumentException($"Tag with id {tagInfo.id} already exists.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RemoveTag(string tagID)
        {
            return tags.Remove(tagID);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool RemoveTag(string tagID, out GameTagInfo tagInfo)
        {
            return tags.Remove(tagID, out tagInfo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetTag(string tagID, out GameTagInfo tagInfo)
        {
            return tags.TryGetValue(tagID, out tagInfo);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetTagWithWarning(string tagID, out GameTagInfo tagInfo)
        {
            if (tags.TryGetValue(tagID, out tagInfo) == false)
            {
                Debugger.LogWarning($"Tag with id {tagID} does not exist.");
                return false;
            }
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasTag(string tagID)
        {
            return tags.ContainsKey(tagID);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasTagWithWarning(string tagID)
        {
            if (tags.ContainsKey(tagID) == false)
            {
                Debugger.LogWarning($"Tag with id {tagID} does not exist.");
                return false;
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Clear()
        {
            tags.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<GameTagInfo> GetAllTags()
        {
            return tags.Values;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> GetAllTagIDs()
        {
            return tags.Keys;
        }
    }
}