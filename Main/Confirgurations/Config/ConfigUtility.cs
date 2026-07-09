using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public static class ConfigUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Init<TConfig>(this IEnumerable<TConfig> configs) where TConfig : IInitializableConfig
        {
            if (configs == null)
            {
                return;
            }
            
            foreach (var config in configs)
            {
                config.Init();
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InitAndSkipNulls<TConfig>(this IEnumerable<TConfig> configs) where TConfig : IInitializableConfig
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
                
                config.Init();
            }
        }
    }
}