using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public static class CheckableConfigUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckSettings<TConfig>(this IEnumerable<TConfig> configs) where TConfig : ICheckableConfig
        {
            if (configs == null)
            {
                return;
            }
            
            foreach (var config in configs)
            {
                config.CheckSettings();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckSettingsAndSkipNulls<TConfig>(this IEnumerable<TConfig> configs)
            where TConfig : ICheckableConfig
        {
            if (configs == null)
            {
                return;
            }

            foreach (var config in configs)
            {
                if (config.IsUnityNull())
                {
                    continue;
                }
                
                config.CheckSettings();
            }
        }
    }
}