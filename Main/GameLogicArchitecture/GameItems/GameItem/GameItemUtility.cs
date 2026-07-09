using System;
using System.Runtime.CompilerServices;

namespace VMFramework.GameLogicArchitecture
{
    public static class GameItemUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TGameItem GetGameItem<TGameItem>(this IGamePrefab gamePrefab)
            where TGameItem : IGameItem
        {
            return GameItemManager.Instance.Get<TGameItem>(gamePrefab.id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IGameItem GetGameItem(this IGamePrefab gamePrefab)
        {
            return GameItemManager.Instance.Get(gamePrefab.id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TGameItem GetClone<TGameItem>(this TGameItem gameItem, StateCloneHint hint)
            where TGameItem : IGameItem
        {
            if (gameItem == null)
            {
                throw new ArgumentNullException(nameof(gameItem));
            }

            var clone = GameItemManager.Instance.Get<TGameItem>(gameItem.id);

            var stateCloner = clone.StateCloner;

            if (stateCloner == null)
            {
                throw new ArgumentNullException($"{clone} does not have a {nameof(StateCloner)}.");
            }

            clone.StateCloner.CloneFrom(gameItem.StateCloner, hint);

            return clone;
        }
    }
}