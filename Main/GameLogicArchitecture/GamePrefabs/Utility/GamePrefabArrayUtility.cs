using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public static class GamePrefabArrayUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteToIDArray<TGamePrefab>([DisallowNull] this TGamePrefab[,] gamePrefabs,
            ref string[] idArray, Vector2Int size)
            where TGamePrefab : IGamePrefab
        {
            var length = size.Products();
            idArray ??= new string[length];
            int index = 0;
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    idArray[index] = gamePrefabs[i, j]?.id;
                    index++;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadFromIDArray<TGamePrefab>([DisallowNull] this string[] idArray,
            ref TGamePrefab[,] gamePrefabs, Vector2Int size)
            where TGamePrefab : IGamePrefab
        {
            gamePrefabs ??= new TGamePrefab[size.x, size.y];
            int index = 0;
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    gamePrefabs[i, j] = GamePrefabManager.GetGamePrefab<TGamePrefab>(idArray[index]);
                    index++;
                }
            }
        }
    }
}