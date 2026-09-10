using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public static class CheckableConfigUtility
    {
        public static void CheckUniqueIDs<TConfig>(this IEnumerable<TConfig> configs, string owner)
            where TConfig : class, IConfig, IIDOwner<string>
        {
            var ids = new HashSet<string>(System.StringComparer.Ordinal);
            foreach (var config in configs)
            {
                if (config == null || string.IsNullOrWhiteSpace(config.id) || !ids.Add(config.id))
                {
                    throw new System.InvalidOperationException(
                        $"{owner} requires non-null configurations with unique, non-empty IDs. Invalid ID: '{config?.id}'.");
                }
            }
        }

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
