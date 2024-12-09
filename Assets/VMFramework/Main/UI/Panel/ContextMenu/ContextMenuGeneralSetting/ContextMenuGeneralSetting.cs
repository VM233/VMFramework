using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public sealed partial class ContextMenuGeneralSetting : GeneralSetting
    {
        private const string CONTEXT_MENU_CATEGORY = "Context Menu";

        private const string CONTEXT_MENU_ID_BIND_CATEGORY =
            TAB_GROUP_NAME + "/" + CONTEXT_MENU_CATEGORY + "/Context Menu ID Bind";

        [TabGroup(TAB_GROUP_NAME, CONTEXT_MENU_CATEGORY), TitleGroup(CONTEXT_MENU_ID_BIND_CATEGORY)]
        [GamePrefabID(typeof(IContextMenuConfig))]
        [IsNotNullOrEmpty]
        [JsonProperty, SerializeField]
        public string defaultContextMenuID;
        
        [TitleGroup(CONTEXT_MENU_ID_BIND_CATEGORY)]
        [JsonProperty]
        public GameTagBasedConfigs<ContextMenuBindConfig> contextMenuIDBindConfigs = new();

        public override void CheckSettings()
        {
            base.CheckSettings();

            if (defaultContextMenuID.IsNullOrEmpty())
            {
                Debugger.LogWarning($"{nameof(defaultContextMenuID)} is not set.");
            }
            
            contextMenuIDBindConfigs.CheckSettings();
        }

        protected override void OnInit()
        {
            base.OnInit();
            
            contextMenuIDBindConfigs.Init();
        }
    }
}
