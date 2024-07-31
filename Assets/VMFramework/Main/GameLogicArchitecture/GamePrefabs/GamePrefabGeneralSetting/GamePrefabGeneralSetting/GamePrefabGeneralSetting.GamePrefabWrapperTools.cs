﻿#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.GameLogicArchitecture.Editor;
using VMFramework.OdinExtensions;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GamePrefabGeneralSetting
    {
        #region Collect All Game Prefab Wrappers

        [Button(ButtonSizes.Medium), TabGroup(TAB_GROUP_NAME, INITIAL_GAME_PREFABS_CATEGORY)]
        private void CollectAllGamePrefabWrappers()
        {
            foreach (var wrapper in GamePrefabWrapperQueryTools.GetGamePrefabWrappers(BaseGamePrefabType))
            {
                AddToInitialGamePrefabWrappers(wrapper);
            }

            this.EnforceSave();
        }

        #endregion

        #region Game Prefab Create

        [Button(ButtonSizes.Medium, ButtonStyle.FoldoutButton),
         TabGroup(TAB_GROUP_NAME, INITIAL_GAME_PREFABS_CATEGORY)]
        private void CreateGamePrefab([IsNotNullOrEmpty, IsUncreatedGamePrefabID] string gamePrefabID,
            GamePrefabWrapperType wrapperType)
        {
            gamePrefabID.AssertIsNotNullOrWhiteSpace(nameof(gamePrefabID));

            var gamePrefabTypes = BaseGamePrefabType.GetDerivedInstantiableClasses(true);

            new TypeSelector(gamePrefabTypes, selectedType =>
            {
                var wrapper =
                    GamePrefabWrapperCreator.CreateGamePrefabWrapper(gamePrefabID, selectedType, wrapperType);

                if (wrapper == null)
                {
                    return;
                }
                
                AddDefaultGameTypeToGamePrefabWrapper(wrapper);

                wrapper.OpenInNewInspector();
            }).ShowInPopup();
        }

        #endregion
    }
}
#endif