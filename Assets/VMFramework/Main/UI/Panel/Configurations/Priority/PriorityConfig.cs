using System;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    [PreviewComposite]
    public abstract partial class PriorityConfig : BaseConfig
    {
        [JsonProperty, SerializeField]
        [EnumToggleButtons]
        private PriorityType priorityType;
        
        [ShowIf(nameof(priorityType), PriorityType.Preset)]
#if UNITY_EDITOR
        [ValueDropdown(nameof(GetPriorityNameList))]
#endif
        [JsonProperty, SerializeField]
        private string presetID;

        [ShowIf(nameof(priorityType), PriorityType.Custom)]
        [JsonProperty, SerializeField]
        private int priority;

        protected PriorityConfig() : this(0)
        {
            
        }

        protected PriorityConfig(string presetID)
        {
            this.priorityType = PriorityType.Preset;
            this.presetID = presetID;
        }
        
        protected PriorityConfig(int priority)
        {
            this.priorityType = PriorityType.Custom;
            this.priority = priority;
        }

        public override string ToString()
        {
            return priorityType switch
            {
                PriorityType.Preset => presetID,
                PriorityType.Custom => priority.ToString(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        protected abstract bool TryGetPriorityFromPreset(string presetID, out int priority);

        public int GetPriority()
        {
            switch (priorityType)
            {
                case PriorityType.Preset:
                    if (presetID.IsNullOrEmpty())
                    {
                        Debugger.LogWarning("No Tooltip Priority Preset ID set.");
                        return 0;
                    }
                    
                    if (TryGetPriorityFromPreset(presetID, out int priorityInConfig))
                    {
                        return priorityInConfig;
                    }
                    
                    Debugger.LogWarning($"No Tooltip Priority Preset found with ID: {presetID}");
                    return 0;
                case PriorityType.Custom:
                    return priority;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static implicit operator int(PriorityConfig config)
        {
            return config.GetPriority();
        }
    }
}