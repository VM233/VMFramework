using System;
using VMFramework.GameLogicArchitecture;
using VMFramework.Core;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.OdinExtensions;
using Object = UnityEngine.Object;

namespace VMFramework.UI
{
    public class UIPanelConfig : GamePrefab, IUIPanelConfig, IPrefabConfig
    {
        public override string IDSuffix => "ui";

        public override Type GameItemType => typeof(UIPanel);

        [PropertyTooltip("UI With larger Sorting Order will cover smaller ones")]
        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [CommonPreset(UIPanelGeneralSetting.SORTING_ORDER_PRESET_KEY)]
        [JsonProperty]
        public int sortingOrder;

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [JsonProperty]
        public bool isUnique = true;

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [AssetsOnly]
        [Required]
        [ComponentRequired(nameof(GameItemType))]
        [JsonProperty]
        public GameObject prefab;

        #region Interface Implementation

        bool IUIPanelConfig.IsUnique => isUnique;

        int IUIPanelConfig.SortingOrder => sortingOrder;

        GameObject IPrefabProvider.Prefab => prefab;

        void IPrefabConfig.SetPrefab(GameObject prefab) => this.prefab = prefab;

        #endregion

        protected override void OnInit()
        {
            base.OnInit();

            if (isUnique)
            {
                gameItemPrewarmCount = 0;
            }
        }

        IGameItem IGamePrefab.GenerateGameItem()
        {
            var newGameObject = Object.Instantiate(prefab, UIPanelManager.Instance.UIContainer);
            var gameItem = (IGameItem)newGameObject.GetComponent(GameItemType);
            
            return gameItem;
        }
    }
}
