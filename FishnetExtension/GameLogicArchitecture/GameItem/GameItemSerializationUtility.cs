#if FISHNET
using System.Runtime.CompilerServices;
using FishNet.CodeGenerating;
using FishNet.Serializing;

namespace VMFramework.GameLogicArchitecture
{
    [NotSerializer]
    public static class GameItemSerializationUtility
    {
        /// <summary>
        /// Fishnet的网络byte流写入
        /// </summary>
        [NotSerializer]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteGameItem(this Writer writer, IGameItem gameItem)
        {
            if (gameItem == null)
            {
                writer.WriteString(IGamePrefab.NULL_ID);
            }
            else
            {
                writer.WriteString(gameItem.id);
                
                gameItem.NetworkSerializer?.WriteFishnet(writer);
            }
        }

        /// <summary>
        /// Fishnet的网络byte流读出
        /// </summary>
        [NotSerializer]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TGameItem ReadGameItem<TGameItem>(Reader reader) where TGameItem : IGameItem
        {
            var id = reader.ReadStringAllocated();

            if (id == IGamePrefab.NULL_ID)
            {
                return default;
            }

            var gameItem = GameItemManager.Instance.Get<TGameItem>(id);

            gameItem.NetworkSerializer?.ReadFishnet(reader);

            return gameItem;
        }
    }
}
#endif