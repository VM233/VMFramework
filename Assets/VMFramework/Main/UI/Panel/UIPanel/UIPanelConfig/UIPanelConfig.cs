using System;
using System.Collections.Generic;
using VMFramework.GameLogicArchitecture;
using VMFramework.Core;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core.Linq;
using VMFramework.OdinExtensions;
using Object = UnityEngine.Object;

namespace VMFramework.UI
{
    public partial class UIPanelConfig : LocalizedGamePrefab, IUIPanelConfig
    {
        protected const string MODIFIERS_CATEGORY = "Modifiers";
        
        public override string IDSuffix => "ui";

        public override Type GameItemType => typeof(UIPanel);
        
        [PropertyTooltip("UI With larger Sorting Order will cover smaller ones")]
        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [JsonProperty]
        public SortingOrderConfig sortingOrderConfig = new(UIPanelGeneralSetting.DEFAULT_SORTING_ORDER_ID);

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [JsonProperty]
        public bool isUnique = true;

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [JsonProperty]
        public bool hasPrefab;

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [ShowIf(nameof(hasPrefab))]
        [AssetsOnly]
        [Required]
        [RequiredComponent(nameof(GameItemType))]
        [JsonProperty]
        public GameObject prefab;

        [TabGroup(TAB_GROUP_NAME, MODIFIERS_CATEGORY)]
        [JsonProperty]
        public List<IPanelModifierConfig> modifiersConfigs = new();

        #region Interface Implementation

        bool IUIPanelConfig.IsUnique => isUnique;

        int IUIPanelConfig.SortingOrder => sortingOrderConfig.GetPriority();

        public IReadOnlyList<IPanelModifierConfig> ModifiersConfigs => modifiersConfigs;

        #endregion

        public override void CheckSettings()
        {
            base.CheckSettings();

            GameItemType.AssertIsDerivedFrom(typeof(IUIPanel), true, false);
            
            sortingOrderConfig.CheckSettings();
            
            if (isUnique)
            {
                if (gameItemPrewarmCount > 0)
                {
                    Debugger.LogError($"{this} is marked as unique but has a " +
                                      $"{nameof(gameItemPrewarmCount)} of {gameItemPrewarmCount}." +
                                      $"It should be set to 0.");
                }
            }
            
            modifiersConfigs.CheckSettings();
        }

        protected override void OnInit()
        {
            base.OnInit();
            
            sortingOrderConfig.Init();

            if (isUnique)
            {
                gameItemPrewarmCount = 0;
            }
        }

        IGameItem IGamePrefab.GenerateGameItem()
        {
            IGameItem gameItem;
            if (hasPrefab)
            {
                var newGameObject = Object.Instantiate(prefab, UIPanelManager.UIContainer);
                gameItem = (IGameItem)newGameObject.GetComponent(GameItemType);
            }
            else
            {
                var newGameObject = new GameObject(name);
                newGameObject.transform.SetParent(UIPanelManager.UIContainer);
                gameItem = (IGameItem)newGameObject.AddComponent(GameItemType);
            }
            
            return gameItem;
        }

        void IGamePrefabProvider.GetGamePrefabs(ICollection<IGamePrefab> gamePrefabsCollection)
        {
            if (ModifiersConfigs.IsNullOrEmpty())
            {
                return;
            }
            
            foreach (var (index, processor) in ModifiersConfigs.Enumerate())
            {
                if (processor != null)
                {
                    processor.id = id + "_" + index;
                }
                
                gamePrefabsCollection.Add(processor);
            }
        }
    }
}
