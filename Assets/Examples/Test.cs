// using System.Runtime.CompilerServices;
// using FishNet.CodeGenerating;
// using FishNet.Serializing;
//
// namespace Test
// {
//     public interface IGameItem
//     {
//         
//     }
//
//     public static class GameItemSerializationUtility
//     {
//         [NotSerializer]
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         public static void WriteGameItem(this Writer writer, IGameItem gameItem)
//         {
//             // Some code
//         }
//
//         [NotSerializer]
//         [MethodImpl(MethodImplOptions.AggressiveInlining)]
//         public static TGameItem ReadGameItem<TGameItem>(this Reader reader)
//             where TGameItem : IGameItem
//         {
//             throw new System.NotImplementedException();
//         }
//     }
// }