using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public abstract partial class GamePrefabGeneralSetting : GeneralSetting, IGamePrefabsProvider
    {
        #region Categories

        protected const string INITIAL_GAME_PREFABS_CATEGORY = "Initial GamePrefabs";

        protected const string GAME_TYPE_CATEGORY = "Game Type";

        #endregion
        
        #region Setting Metadata

        [TabGroup(TAB_GROUP_NAME, METADATA_CATEGORY)]
        [ShowInInspector]
        public virtual string GamePrefabName
        {
            get
            {
                if (BaseGamePrefabType.IsInterface == false)
                {
                    return BaseGamePrefabType.Name;
                }

                if (BaseGamePrefabType.Name.StartsWith("I"))
                {
                    return BaseGamePrefabType.Name[1..];
                }
                
                return BaseGamePrefabType.Name;
            }
        }
        
        [TabGroup(TAB_GROUP_NAME, METADATA_CATEGORY)]
        [ShowInInspector]
        public abstract Type BaseGamePrefabType { get; }

        #endregion

#if UNITY_EDITOR
        [LabelText("Initial GamePrefab"),
         TabGroup(TAB_GROUP_NAME, INITIAL_GAME_PREFABS_CATEGORY, SdfIconType.Info, TextColor = "blue")]
        [OnCollectionChanged(nameof(OnInitialGamePrefabProvidersChanged))]
        [Searchable]
#endif
        public List<IGamePrefabsProvider> initialGamePrefabProviders = new();

        public void RefreshInitialGamePrefabProviders()
        {
            initialGamePrefabProviders ??= new();

            initialGamePrefabProviders.RemoveAll(wrapper => wrapper.IsUnityNull());
        }

        #region Initial Game Prefab Provider

        public void GetGamePrefabs(ICollection<IGamePrefab> gamePrefabsCollection)
        {
            RefreshInitialGamePrefabProviders();
            
            foreach (var wrapper in initialGamePrefabProviders)
            {
                wrapper.GetGamePrefabs(gamePrefabsCollection);
            }
        }

        #endregion
    }
}
