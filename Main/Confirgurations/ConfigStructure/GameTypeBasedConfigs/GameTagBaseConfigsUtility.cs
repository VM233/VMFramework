using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Configuration
{
    public static class GameTagBaseConfigsUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TConfig GetConfigRuntime<TConfig, TEnumerable>(
            this IGameTagBasedConfigs<TConfig> gameTagBasedConfigs, TEnumerable gameTags)
            where TConfig : IConfig
            where TEnumerable : IEnumerable<string>
        {
            if (gameTags == null)
            {
                return default;
            }
            
            foreach (var gameTag in gameTags)
            {
                if (gameTagBasedConfigs.TryGetConfigRuntime(gameTag, out var config))
                {
                    return config;
                }
            }

            return default;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetConfigRuntime<TConfig, TEnumerable>(
            this IGameTagBasedConfigs<TConfig> gameTagBasedConfigs, TEnumerable gameTags, out TConfig config)
            where TConfig : IConfig
            where TEnumerable : IEnumerable<string>
        {
            if (gameTags == null)
            {
                config = default;
                return false;
            }
            
            foreach (var gameTag in gameTags)
            {
                if (gameTagBasedConfigs.TryGetConfigRuntime(gameTag, out config))
                {
                    return true;
                }
            }

            config = default;
            return false;
        }
    }
}