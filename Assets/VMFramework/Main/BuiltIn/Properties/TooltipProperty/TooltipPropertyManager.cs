using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Properties
{
    public static class TooltipPropertyManager
    {
        private static TooltipPropertyGeneralSetting Setting => BuiltInModulesSetting.TooltipPropertyGeneralSetting;
        
        private static readonly Dictionary<string, List<InstanceTooltipPropertyConfigRuntime>>
            tooltipPropertyConfigsRuntimeDict = new();

        public static IEnumerable<InstanceTooltipPropertyConfigRuntime> GetTooltipPropertyConfigsRuntime(
            string gamePrefabID)
        {
            if (tooltipPropertyConfigsRuntimeDict.TryGetValue(gamePrefabID, out var result))
            {
                return result;
            }

            return Enumerable.Empty<InstanceTooltipPropertyConfigRuntime>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddTooltipPropertyConfigRuntime(string gamePrefabID,
            InstanceTooltipPropertyConfigRuntime tooltipPropertyConfigRuntime)
        {
            if (tooltipPropertyConfigsRuntimeDict.TryGetValue(gamePrefabID, out var result))
            {
                result.Add(tooltipPropertyConfigRuntime);
            }
            else
            {
                result = new List<InstanceTooltipPropertyConfigRuntime> { tooltipPropertyConfigRuntime };
                tooltipPropertyConfigsRuntimeDict.Add(gamePrefabID, result);
            }
        }

        public static IEnumerable<InstanceTooltipPropertyConfigRuntime> GetTooltipPropertyConfigsRuntime(
            Type currentInstanceType)
        {
            foreach (var (instanceType, _) in Setting.tooltipPropertyConfigs)
            {
                if (instanceType == null)
                {
                    throw new NullReferenceException(
                        $"There are null values in the {nameof(Setting.tooltipPropertyConfigs)} list.");
                }
            }

            var result = new List<InstanceTooltipPropertyConfigRuntime>();

            foreach (var (instanceType, tooltipPropertyConfig) in Setting.tooltipPropertyConfigs)
            {
                if (currentInstanceType.IsDerivedFrom(instanceType, true))
                {
                    foreach (var configRuntime in tooltipPropertyConfig.tooltipPropertyConfigsRuntime)
                    {
                        result.Add(configRuntime);
                    }
                }
            }

            return result.Distinct(config => config.property);
        }
    }
}