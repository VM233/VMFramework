#if UNITY_EDITOR
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Editor.BatchProcessor
{
    internal sealed class CreateGamePrefabWrapperUnit : GamePrefabRegisterUnit<IGamePrefab>
    {
        protected override string ProcessButtonName => "Create Game Prefab Wrapper";

        protected override bool TryGetAsset(object obj, out IGamePrefab asset)
        {
            if (base.TryGetAsset(obj, out asset))
            {
                return true;
            }

            if (obj.TryGetComponent(out IGamePrefab gamePrefab))
            {
                asset = gamePrefab;
                return true;
            }
            
            return false;
        }

        protected override IGamePrefab OnProcessAsset(IGamePrefab asset)
        {
            if (asset.id.IsNullOrWhiteSpace())
            {
                UnityEngine.Debug.LogWarning($"{asset.GetType().Name} does not have an ID. Skipping.");
                return null;
            }
            
            if (checkUnique)
            {
                if (GamePrefabManager.ContainsGamePrefab(asset.id))
                {
                    return null;
                }
            }
            
            return asset;
        }
    }
}
#endif