using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public sealed partial class ContainerSlotDistributorConfig : BaseConfig
    {
        [VisualElementName]
        public string parentName;

        public bool isFinite = true;
        
        [HideIf(nameof(isFinite))]
        public int startSlotIndex;
        
        [ShowIf(nameof(isFinite))]
        public RangeIntegerConfig slotIndexRange = new(0, 0);

        [ShowIf(nameof(isFinite))]
        public bool autoFill = true;

        [EnableIf(nameof(autoFill))]
        public bool hasCustomContainer;
        
        [ShowIf(nameof(hasCustomContainer))]
        [EnableIf(nameof(autoFill))]
        [VisualElementName]
        public string customContainerName;
        
        public bool slotsDisplayNoneIfNull;
        
        public string ContainerName => hasCustomContainer? customContainerName : parentName;

        public int StartIndex => isFinite ? slotIndexRange.min : startSlotIndex;
        
        public int Count => isFinite ? slotIndexRange.Count : int.MaxValue;
    }
}