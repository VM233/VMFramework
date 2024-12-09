using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using VMFramework.Configuration;

namespace VMFramework.UI
{
    public abstract partial class ContainerPanelModifierBaseConfig : PanelModifierConfig
    {
        public override Type GameItemType => typeof(ContainerPanelModifierBase);

        [TabGroup(TAB_GROUP_NAME, MODIFIER_CATEGORY)]
        [JsonProperty]
        public List<ContainerSlotDistributorConfig> slotDistributorConfigs = new();
        
        public override void CheckSettings()
        {
            base.CheckSettings();

            slotDistributorConfigs.CheckSettings();
        }

        protected override void OnInit()
        {
            base.OnInit();

            slotDistributorConfigs.Init();
        }
    }
}