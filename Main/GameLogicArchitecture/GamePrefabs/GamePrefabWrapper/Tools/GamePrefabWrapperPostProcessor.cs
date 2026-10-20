#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VMFramework.Procedure.Editor;

namespace VMFramework.GameLogicArchitecture.Editor
{
    internal sealed class GamePrefabWrapperPostProcessor : AssetPostprocessor
    {
        private static readonly HashSet<string> providerImports = new(StringComparer.Ordinal);
        private static bool restoreAutoRegistered;

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            AssemblyReloadEvents.beforeAssemblyReload += Cancel;
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.ExitingEditMode) Cancel();
            };
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (!EditorInitializer.IsInitialized || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            bool changed = false;
            foreach (string path in importedAssets.Concat(movedAssets))
            {
                Type type = AssetDatabase.GetMainAssetTypeAtPath(path);
                bool nativeProvider = TryGetProvider(path, type, out _, out _);
                if (nativeProvider) providerImports.Add(path);
                changed |= nativeProvider || GamePrefabWrapperInitializeUtility.OwnsAssetPath(path) ||
                    type != null && (typeof(GamePrefabWrapper).IsAssignableFrom(type) ||
                    typeof(IGeneralSetting).IsAssignableFrom(type) || typeof(IGlobalSettingFile).IsAssignableFrom(type));
            }
            bool removed = deletedAssets.Any(GamePrefabWrapperInitializeUtility.OwnsRemovedAssetPath);
            changed |= removed || movedFromAssetPaths.Any(GamePrefabWrapperInitializeUtility.OwnsRemovedAssetPath);
            if (!changed) return;
            restoreAutoRegistered |= removed;
            EditorApplication.delayCall -= Publish;
            EditorApplication.delayCall += Publish;
        }

        private static bool TryGetProvider(string path, Type type, out IGamePrefab prefab,
            out IGamePrefabsProvider provider)
        {
            prefab = null; provider = null;
            if (type != typeof(GameObject)) return false;
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return asset.TryGetComponent(out prefab) && asset.TryGetComponent(out provider);
        }

        private static void Publish()
        {
            string[] imports = providerImports.ToArray();
            bool restore = restoreAutoRegistered;
            Cancel();
            foreach (string path in imports)
                if (TryGetProvider(path, AssetDatabase.GetMainAssetTypeAtPath(path), out var prefab, out var provider) &&
                    prefab.TryGetGamePrefabGeneralSetting(out var setting))
                    setting.AddToInitialGamePrefabProviders(provider);
            GamePrefabWrapperInitializeUtility.Refresh();
            if (restore) GamePrefabWrapperInitializeUtility.CreateAutoRegisterGamePrefabs();
        }

        private static void Cancel()
        {
            EditorApplication.delayCall -= Publish;
            providerImports.Clear();
            restoreAutoRegistered = false;
        }
    }
}
#endif
