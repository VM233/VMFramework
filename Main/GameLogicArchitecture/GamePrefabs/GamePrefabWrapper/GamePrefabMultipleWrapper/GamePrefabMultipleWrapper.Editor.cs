#if UNITY_EDITOR
using System.Linq;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.Core.Linq;
using VMFramework.GameLogicArchitecture.Editor;
using VMFramework.OdinExtensions;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GamePrefabMultipleWrapper
    {
        private void OnGamePrefabsChanged()
        {
            GamePrefabWrapperInitializeUtility.Refresh();
            this.EnforceSave();
        }

        [Button(ButtonSizes.Medium, ButtonStyle.FoldoutButton)]
        private void CreateGamePrefab([IsNotNullOrEmpty, IsUncreatedGamePrefabID] string gamePrefabID)
        {
            gamePrefabID.AssertIsNotNullOrWhiteSpace(nameof(gamePrefabID));

            var baseGamePrefabType = typeof(IGamePrefab);

            if (gamePrefabs.IsNullOrEmpty() == false)
            {
                var gamePrefab = gamePrefabs.FirstOrDefault(gamePrefab => gamePrefab != null);

                if (gamePrefab != null)
                {
                    if (gamePrefab.TryGetGamePrefabGeneralSettingWithWarning(out var generalSetting))
                    {
                        baseGamePrefabType = generalSetting.BaseGamePrefabType;
                    }
                }
            }
            
            var gamePrefabTypes = baseGamePrefabType.GetDerivedInstantiableClasses(true);

            new TypeSelector(gamePrefabTypes, selectedType =>
            {
                var gamePrefab = GamePrefabWrapperCreator.CreateDefaultGamePrefab(gamePrefabID, selectedType);
                gamePrefabs.Add(gamePrefab);
                
                GamePrefabWrapperInitializeUtility.Refresh();
                this.EnforceSave();
            }).ShowInPopup();
        }
    }
}
#endif