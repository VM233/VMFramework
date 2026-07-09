#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;
using VMFramework.Core.Editor;
using VMFramework.Procedure.Editor;

namespace VMFramework.GameLogicArchitecture.Editor
{
    internal sealed class GamePrefabWrapperPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (EditorInitializer.IsInitialized == false)
            {
                return;
            }

            EditorApplication.delayCall += () =>
            {
                foreach (var importedAssetPath in importedAssets)
                {
                    if (importedAssetPath.TryGetAssetByPath<GameObject>(out var asset) == false)
                    {
                        continue;
                    }
                    
                    if (asset.TryGetComponent(out IGamePrefab gamePrefab) == false)
                    {
                        continue;
                    }

                    if (asset.TryGetComponent(out IGamePrefabsProvider gamePrefabProvider) == false)
                    {
                        continue;
                    }

                    if (gamePrefab.TryGetGamePrefabGeneralSetting(out var gamePrefabGeneralSetting))
                    {
                        gamePrefabGeneralSetting.AddToInitialGamePrefabProviders(gamePrefabProvider);
                    }
                }
                
                if (importedAssets.Any(assetPath => assetPath.EndsWith(".asset")))
                {
                    GamePrefabWrapperInitializeUtility.Refresh();
                }
                else if (deletedAssets.Any(assetPath => assetPath.EndsWith(".asset")))
                {
                    GamePrefabWrapperInitializeUtility.Refresh();
                    GamePrefabWrapperInitializeUtility.CreateAutoRegisterGamePrefabs();
                }
            };
        }
    }
}
#endif