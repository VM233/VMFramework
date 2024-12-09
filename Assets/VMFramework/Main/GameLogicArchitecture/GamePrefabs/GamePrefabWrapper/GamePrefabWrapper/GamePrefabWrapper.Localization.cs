#if UNITY_EDITOR
using VMFramework.Core.Editor;
using VMFramework.Core.Pools;
using VMFramework.Localization;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GamePrefabWrapper : ILocalizedStringOwnerConfig
    {
        public void AutoConfigureLocalizedString(LocalizedStringAutoConfigSettings settings)
        {
            bool isDirty = false;

            var gamePrefabsCache = ListPool<IGamePrefab>.Default.Get();
            gamePrefabsCache.Clear();
            GetGamePrefabs(gamePrefabsCache);
            
            foreach (var gamePrefab in gamePrefabsCache)
            {
                if (gamePrefab is ILocalizedStringOwnerConfig localizedStringOwnerConfig)
                {
                    localizedStringOwnerConfig.AutoConfigureLocalizedString(settings);
                    isDirty = true;
                }
            }
            
            gamePrefabsCache.ReturnToDefaultPool();

            if (isDirty)
            {
                if (settings.save)
                {
                    this.EnforceSave();
                }
                else
                {
                    this.SetEditorDirty();
                }
            }
        }

        public void SetKeyValueByDefault()
        {
            var gamePrefabsCache = ListPool<IGamePrefab>.Default.Get();
            gamePrefabsCache.Clear();
            GetGamePrefabs(gamePrefabsCache);
            
            foreach (var gamePrefab in gamePrefabsCache)
            {
                if (gamePrefab is ILocalizedStringOwnerConfig localizedStringOwnerConfig)
                {
                    localizedStringOwnerConfig.SetKeyValueByDefault();
                }
            }
            
            gamePrefabsCache.ReturnToDefaultPool();
        }
    }
}
#endif