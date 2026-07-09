#if FISHNET
using System;
using System.Runtime.CompilerServices;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Network
{
    public static class UUIDGameItemFactory
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TGameItem Get<TGameItem>(string id, Guid? guid = null) where TGameItem : IGameItem
        {
            var gameItem = GameItemManager.Instance.Get<TGameItem>(id);
            guid ??= Guid.NewGuid();
            gameItem.UUIDOwner.TrySetUUIDAndRegister(guid.Value);
            return gameItem;
        }
    }
}
#endif