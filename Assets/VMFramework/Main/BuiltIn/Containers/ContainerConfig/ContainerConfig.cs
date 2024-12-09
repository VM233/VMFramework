using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Containers
{
    public class ContainerConfig : GamePrefab, IContainerConfig
    {
        public override string IDSuffix => "container";

        public override Type GameItemType => typeof(Container);

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
    }
}