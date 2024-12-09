using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.GameLogicArchitecture
{
    public abstract partial class GamePrefabGeneralSetting : GeneralSetting, IGamePrefabProvider
    {
        public const string UNDEFINED_GAME_ITEM_NAME = "Undefined Game Item Name";
        
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
        public virtual string GameItemName { get; } = UNDEFINED_GAME_ITEM_NAME;
        
        [TabGroup(TAB_GROUP_NAME, METADATA_CATEGORY)]
        [ShowInInspector]
        public abstract Type BaseGamePrefabType { get; }

        #endregion

#if UNITY_EDITOR
        [LabelText("Initial GamePrefab"),
         TabGroup(TAB_GROUP_NAME, INITIAL_GAME_PREFABS_CATEGORY, SdfIconType.Info, TextColor = "blue")]
        [OnCollectionChanged(nameof(OnInitialGamePrefabWrappersChanged))]
        [Searchable]
#endif
        [SerializeField]
        private List<GamePrefabWrapper> initialGamePrefabWrappers = new();

        public void RefreshInitialGamePrefabWrappers()
        {
            initialGamePrefabWrappers ??= new();

            initialGamePrefabWrappers.RemoveAll(wrapper => wrapper == null);
        }

        #region Initial Game Prefab Provider

        public void GetGamePrefabs(ICollection<IGamePrefab> gamePrefabsCollection)
        {
            RefreshInitialGamePrefabWrappers();
            
            foreach (var wrapper in initialGamePrefabWrappers)
            {
                wrapper.GetGamePrefabs(gamePrefabsCollection);
            }
        }

        #endregion
    }
}
