using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using VMFramework.GameEvents;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public partial class UIToolkitContextMenuConfig : UIToolkitPanelConfig, IContextMenuConfig
    {
        public const string CONTEXT_MENU_CATEGORY = "Context Menu";

        public override Type GameItemType => typeof(UIToolkitContextMenu);

        [TabGroup(TAB_GROUP_NAME, CONTEXT_MENU_CATEGORY)]
        [VisualElementName]
        [JsonProperty]
        public string contextMenuEntryContainerName;

        [TabGroup(TAB_GROUP_NAME, CONTEXT_MENU_CATEGORY)]
        public Sprite entrySelectedIcon;

        [SuffixLabel("If Only One Entry"), TabGroup(TAB_GROUP_NAME, CONTEXT_MENU_CATEGORY)]
        [JsonProperty]
        public bool autoExecuteIfOnlyOneEntry = true;

        [TabGroup(TAB_GROUP_NAME, CONTEXT_MENU_CATEGORY)]
        [JsonProperty]
        public MouseButtonType clickMouseButtonType = MouseButtonType.LeftButton;

        [TabGroup(TAB_GROUP_NAME, CONTEXT_MENU_CATEGORY)]
        [GamePrefabID(typeof(IGameEventConfig))]
        [JsonProperty]
        public List<string> gameEventIDsToClose = new();

        public override void CheckSettings()
        {
            base.CheckSettings();

            isUnique = true;
        }
    }
}
