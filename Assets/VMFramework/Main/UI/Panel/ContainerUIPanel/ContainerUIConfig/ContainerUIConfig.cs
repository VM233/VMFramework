using System;
using System.Collections.Generic;
using VMFramework.Core;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.Containers;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public partial class ContainerUIConfig : UIToolkitPanelConfig, IContainerUIConfig
    {
        protected const string CONTAINER_CATEGORY = "Container UI";

        public override Type GameItemType => typeof(ContainerUIPanel);

        [TabGroup(TAB_GROUP_NAME, CONTAINER_CATEGORY, SdfIconType.Box, TextColor = "magenta")]
        [JsonProperty]
        public int containerUIPriority = 0;

        #region Interface Implementation

        int IContainerUIConfig.ContainerUIPriority => containerUIPriority;

        #endregion
    }
}