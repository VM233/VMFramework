using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    [Serializable]
    public class ContainerSlotNameBindingConfig
    {
        [VisualElementName(typeof(SlotVisualElement))]
        [IsNotNullOrEmpty]
        public string slotName;

        [MinValue(0)]
        public int slotIndex;
    }

    [Serializable]
    public class ContainerSlotDistributorConfig
    {
        [VisualElementName]
        public string parentName;

        public SlotFindMethod findMethod = SlotFindMethod.Default;
        
        [ShowIf(nameof(findMethod), SlotFindMethod.ByName)]
        [VisualElementName]
        [IsNotNullOrEmpty]
        public string slotName;

        [ShowIf(nameof(findMethod), SlotFindMethod.BySlotNames)]
        public List<ContainerSlotNameBindingConfig> slotNameBindings = new();
        
        public bool removeExtraSlots = true;

        [HideIf(nameof(UsesSlotNameBindings))]
        public bool isFinite = true;
        
        [ShowIf(nameof(UsesStartSlotIndex))]
        public int startSlotIndex;
        
        [ShowIf(nameof(UsesSlotIndexRange))]
        public RangeInteger slotIndexRange = new(0, 0);

        [ShowIf(nameof(UsesSlotIndexRange))]
        public bool autoFill;

        [HideIf(nameof(UsesSlotNameBindings))]
        [EnableIf(nameof(autoFill))]
        public bool hasCustomContainer;
        
        [ShowIf(nameof(UsesCustomContainerName))]
        [EnableIf(nameof(autoFill))]
        [VisualElementName]
        public string customContainerName;

        private bool UsesSlotNameBindings => findMethod == SlotFindMethod.BySlotNames;

        private bool UsesStartSlotIndex => UsesSlotNameBindings == false && isFinite == false;

        private bool UsesSlotIndexRange => UsesSlotNameBindings == false && isFinite;

        private bool UsesCustomContainerName => UsesSlotNameBindings == false && hasCustomContainer;
        
        public string ContainerName => hasCustomContainer? customContainerName : parentName;

        public int StartIndex => isFinite ? slotIndexRange.min : startSlotIndex;
        
        public int Count => isFinite ? slotIndexRange.Count : int.MaxValue;
    }
}
