using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    public abstract partial class GamePrefabGeneralSetting : GeneralSettingBase, IInitialGamePrefabProvider
    {
        #region Categories

        protected const string INITIAL_GAME_PREFABS_CATEGORY = "初始GamePrefab";

        protected const string GAME_TYPE_CATEGORY = "游戏种类";

        #endregion
        
        #region Setting Metadata

        public abstract string prefabName { get; }

        [LabelText("GamePrefab文件夹路径"), TabGroup(TAB_GROUP_NAME, GENERAL_SETTING_METADATA_CATEGORY)]
        [ShowInInspector]
        public string gamePrefabDirectoryPath =>
            ConfigurationPath.GAME_PREFAB_DIRECTORY_PATH + "/" + prefabName;
        
        [LabelText("GamePrefab类型"), TabGroup(TAB_GROUP_NAME, GENERAL_SETTING_METADATA_CATEGORY)]
        [ShowInInspector]
        public abstract Type baseGamePrefabType { get; }

        #endregion

#if UNITY_EDITOR
        [LabelText("初始GamePrefab"), TabGroup(TAB_GROUP_NAME, INITIAL_GAME_PREFABS_CATEGORY)]
        [OnCollectionChanged(nameof(OnInitialGamePrefabWrappersChanged))]
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