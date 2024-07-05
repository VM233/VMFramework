using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.GameLogicArchitecture
{
    public abstract partial class GamePrefabGeneralSetting : GeneralSetting, IInitialGamePrefabProvider
    {
        public const string UNDEFINED_GAME_ITEM_NAME = "Undefined Game Item Name";
        
        #region Categories

        protected const string INITIAL_GAME_PREFABS_CATEGORY = "Initial GamePrefabs";

        protected const string GAME_TYPE_CATEGORY = "Game Type";

        #endregion
        
        #region Setting Metadata

        [TabGroup(TAB_GROUP_NAME, METADATA_CATEGORY)]
        [ShowInInspector]
        public virtual string gamePrefabName => baseGamePrefabType.Name;

        [TabGroup(TAB_GROUP_NAME, METADATA_CATEGORY)]
        [ShowInInspector]
        public virtual string gameItemName { get; } = UNDEFINED_GAME_ITEM_NAME;
        
        [TabGroup(TAB_GROUP_NAME, METADATA_CATEGORY)]
        [ShowInInspector]
        public abstract Type baseGamePrefabType { get; }

        #endregion

#if UNITY_EDITOR
        [LabelText("Initial GamePrefab"),
         TabGroup(TAB_GROUP_NAME, INITIAL_GAME_PREFABS_CATEGORY, SdfIconType.Info, TextColor = "blue")]
        [OnCollectionChanged(nameof(OnInitialGamePrefabWrappersChanged))]
        [Searchable]
#endif
        [SerializeField]
        private List<GamePrefabWrapper> initialGamePrefabWrappers = new();

        #region Initial Game Prefab Provider

        IEnumerable<IGamePrefab> IInitialGamePrefabProvider.GetInitialGamePrefabs()
        {
            initialGamePrefabWrappers ??= new();
            
            initialGamePrefabWrappers.RemoveAll(wrapper => wrapper == null);
            
            foreach (var wrapper in initialGamePrefabWrappers)
            {
                foreach (var gamePrefab in wrapper.GetGamePrefabs())
                {
                    yield return gamePrefab;
                }
            }
        }

        #endregion
    }
}
