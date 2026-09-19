using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public static class CheckableConfigUtility
    {
        public static void CheckUniqueIDs<TConfig>(this IEnumerable<TConfig> configs, string owner)
            where TConfig : IIDOwner<string>
        {
            var ids = new HashSet<string>(System.StringComparer.Ordinal);
            foreach (var config in configs)
            {
                var id = config is null ? null : config.id;
                if (string.IsNullOrWhiteSpace(id) || !ids.Add(id))
                {
                    throw new System.InvalidOperationException(
                        $"{owner} requires non-null configurations with unique, non-empty IDs. Invalid ID: '{id}'.");
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
