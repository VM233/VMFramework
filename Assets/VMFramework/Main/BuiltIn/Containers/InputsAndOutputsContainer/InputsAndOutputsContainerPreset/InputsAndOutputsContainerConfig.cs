using System;
using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.OdinExtensions;

namespace VMFramework.Containers
{
    public partial class InputsAndOutputsContainerConfig : ContainerConfig
    {
        public override Type GameItemType => typeof(InputsAndOutputsContainer);

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [RangeSlider(0, nameof(MaxSlotIndex))]
        public RangeIntegerConfig inputsRange = new(1, 12);

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [RangeSlider(0, nameof(MaxSlotIndex))]
        public RangeIntegerConfig outputsRange = new(13, 16);
    }
}