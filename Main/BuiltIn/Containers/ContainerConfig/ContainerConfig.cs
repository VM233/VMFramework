using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;
using Object = UnityEngine.Object;

namespace VMFramework.Containers
{
    public class ContainerConfig : GamePrefab, IContainerConfig, IPrefabConfig
    {
        public override string IDSuffix => "container";

        public override Type GameItemType => typeof(Container);

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [ComponentRequired(nameof(GameItemType))]
        [Required]
        public GameObject prefab;

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [JsonProperty]
        public bool hasFixedSize = true;

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [EnableIf(nameof(hasFixedSize))]
        [MinValue(1)]
        [JsonProperty]
        public int fixedSize = 9;

        protected int MaxSlotIndex => hasFixedSize ? fixedSize - 1 : int.MaxValue;

        int? IContainerConfig.Capacity => hasFixedSize ? fixedSize : null;
        
        GameObject IPrefabProvider.Prefab => prefab;

        void IPrefabConfig.SetPrefab(GameObject prefab) => this.prefab = prefab;

        IGameItem IGamePrefab.GenerateGameItem()
        {
            var gameObject = Object.Instantiate(prefab);
            var container = gameObject.GetComponent<IContainer>();
            return container;
        }
    }
}