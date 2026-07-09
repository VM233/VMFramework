#if UNITY_EDITOR
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Editor.BatchProcessor
{
    internal sealed class GeneralGamePrefabRegisterUnit : GamePrefabRegisterUnit<IControllerGameItem>
    {
        protected override string ProcessButtonName => "Register Game Prefab";

        protected override bool TryGetAsset(object obj, out IControllerGameItem asset)
        {
            if (base.TryGetAsset(obj, out asset))
            {
                return true;
            }

            return obj.TryGetComponent(out asset);
        }

        protected override IGamePrefab OnProcessAsset(IControllerGameItem asset)
        {
            if (checkUnique)
            {
                foreach (var config in GamePrefabManager.GetAllGamePrefabs<IPrefabConfig>())
                {
                    if (config.Prefab == asset.gameObject)
                    {
                        UnityEngine.Debug.LogWarning(
                            $"Ignore adding {asset.gameObject.name} as it is already registered in {config}.");
                        return null;
                    }
                }
            }
            
            if (GamePrefabTypeQuery.TryGetGamePrefabTypeByGameItemType(asset.GetType(), out var gamePrefabType) ==
                false)
            {
                UnityEngine.Debug.LogWarning($"Could not find a game prefab type for game item type {asset.GetType()}");
                return null;
            }

            var gamePrefab = (IGamePrefab)gamePrefabType.CreateInstance();

            if (gamePrefab is not IPrefabConfig prefabConfig)
            {
                return null;
            }
            
            prefabConfig.SetPrefab(asset.gameObject);
            
            var id = asset.gameObject.name.MakeWordsSuffix(gamePrefab.IDSuffix).ToSnakeCase();
            gamePrefab.id = id;
            return gamePrefab;
        }
    }
}
#endif