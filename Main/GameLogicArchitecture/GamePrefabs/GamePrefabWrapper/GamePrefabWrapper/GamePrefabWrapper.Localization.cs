#if UNITY_EDITOR
using VMFramework.Core.Pools;
using VMFramework.Localization;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GamePrefabWrapper : ILocalizedStringOwnerConfig
    {
        public virtual void RemoveInvalidLocalizedStrings()
        {
            var gamePrefabsCache = ListPool<IGamePrefab>.Default.Get();
            gamePrefabsCache.Clear();
            GetGamePrefabs(gamePrefabsCache);
            
            foreach (var gamePrefab in gamePrefabsCache)
            {
                if (gamePrefab is ILocalizedStringOwnerConfig localizedStringOwnerConfig)
                {
                    localizedStringOwnerConfig.RemoveInvalidLocalizedStrings();
                }
            }
            
            gamePrefabsCache.ReturnToDefaultPool();
        }

        public virtual void SetDefaultKeyValue(LocalizedStringAutoConfigSettings setting)
        {
            var gamePrefabsCache = ListPool<IGamePrefab>.Default.Get();
            gamePrefabsCache.Clear();
            GetGamePrefabs(gamePrefabsCache);
            
            foreach (var gamePrefab in gamePrefabsCache)
            {
                if (gamePrefab is ILocalizedStringOwnerConfig localizedStringOwnerConfig)
                {
                    localizedStringOwnerConfig.SetDefaultKeyValue(setting);
                }
            }
            
            gamePrefabsCache.ReturnToDefaultPool();
        }
    }
}
#endif