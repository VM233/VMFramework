#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.GameLogicArchitecture
{
    public static class GamePrefabGeneralSettingUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RefreshAllInitialGamePrefabWrappers()
        {
            foreach (var gamePrefabGeneralSetting in GetAllGamePrefabGeneralSettings())
            {
                gamePrefabGeneralSetting.RefreshInitialGamePrefabWrappers();
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<GamePrefabGeneralSetting> GetAllGamePrefabGeneralSettings()
        {
            foreach (var generalSetting in GlobalSettingCollector.GetAllGeneralSettings())
            {
                if (generalSetting is GamePrefabGeneralSetting gamePrefabSetting)
                {
                    yield return gamePrefabSetting;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GamePrefabGeneralSetting GetGamePrefabGeneralSetting(Type gamePrefabType)
        {
            foreach (var gamePrefabSetting in GetAllGamePrefabGeneralSettings())
            {
                if (gamePrefabType.IsDerivedFrom(gamePrefabSetting.BaseGamePrefabType, true))
                {
                    return gamePrefabSetting;
                }
            }
            
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetGamePrefabGeneralSetting(Type gamePrefabType,
            out GamePrefabGeneralSetting gamePrefabSetting)
        {
            gamePrefabSetting = GetGamePrefabGeneralSetting(gamePrefabType);
            return gamePrefabSetting != null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GamePrefabGeneralSetting GetGamePrefabGeneralSetting(this IGamePrefab gamePrefab)
        {
            if (gamePrefab == null)
            {
                return null;
            }
            
            return GetGamePrefabGeneralSetting(gamePrefab.GetType());
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetGamePrefabGeneralSetting(this IGamePrefab gamePrefab,
            out GamePrefabGeneralSetting gamePrefabSetting)
        {
            gamePrefabSetting = GetGamePrefabGeneralSetting(gamePrefab);
            return gamePrefabSetting != null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetGamePrefabGeneralSettingWithWarning(this IGamePrefab gamePrefab,
            out GamePrefabGeneralSetting gamePrefabSetting)
        {
            if (TryGetGamePrefabGeneralSetting(gamePrefab, out gamePrefabSetting))
            {
                return true;
            }
            
            Debug.LogError(
                $"Could not find {nameof(GamePrefabGeneralSetting)} for {gamePrefab?.GetType()}.");
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetGamePrefabGeneralSettings<TGamePrefabs, TGamePrefabGeneralSettings>(
            this TGamePrefabs gamePrefabs, TGamePrefabGeneralSettings gamePrefabGeneralSettings)
            where TGamePrefabs : IEnumerable<IGamePrefab>
            where TGamePrefabGeneralSettings : ICollection<GamePrefabGeneralSetting>
        {
            foreach (var gamePrefab in gamePrefabs)
            {
                if (TryGetGamePrefabGeneralSettingWithWarning(gamePrefab, out var gamePrefabSetting))
                {
                    gamePrefabGeneralSettings.Add(gamePrefabSetting);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetGamePrefabGeneralSettings<TGamePrefabGeneralSettings>(
            this GamePrefabWrapper gamePrefabWrapper, TGamePrefabGeneralSettings gamePrefabGeneralSettings)
            where TGamePrefabGeneralSettings : ICollection<GamePrefabGeneralSetting>
        {
            if (gamePrefabWrapper == null)
            {
                return;
            }
            
            var gamePrefabs = ListPool<IGamePrefab>.Default.Get();
            gamePrefabs.Clear();
            
            gamePrefabWrapper.GetGamePrefabs(gamePrefabs);

            if (gamePrefabs.Count == 0)
            {
                Debugger.LogWarning($"{gamePrefabWrapper.name} has no game prefabs.");
            }
            else
            {
                gamePrefabs.GetGamePrefabGeneralSettings(gamePrefabGeneralSettings);
            }
            
            gamePrefabs.ReturnToDefaultPool();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<ValueDropdownItem> GetGamePrefabGeneralSettingNameList()
        {
            foreach (var gamePrefabSetting in GetAllGamePrefabGeneralSettings())
            {
                yield return new ValueDropdownItem(gamePrefabSetting.GamePrefabName, gamePrefabSetting);
            }
        }
    }
}
#endif