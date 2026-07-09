using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    [Serializable]
    public class IntGeneralMultipleConfig<TConfig, TRuntimeConfig>
    {
        [Serializable]
        public class DictionaryConfig
        {
            public bool useRange = true;

            [ShowIf(nameof(useRange))]
            public RangeInteger range;

            [HideIf(nameof(useRange))]
            [IsNotNullOrEmpty]
            public List<int> intValues = new();

            [IsNotNullOrEmpty]
            public List<TConfig> configs = new();
        }

        public IntGeneralMultipleConfigMode mode = IntGeneralMultipleConfigMode.Array;

        [ShowIf(nameof(mode), IntGeneralMultipleConfigMode.Array)]
        [IsNotNullOrEmpty]
        public List<TConfig> arrayConfigs = new();

        [ShowIf(nameof(mode), IntGeneralMultipleConfigMode.Dictionary)]
        [IsNotNullOrEmpty]
        public List<DictionaryConfig> dictionaryConfigs = new();

        protected readonly Dictionary<int, List<TRuntimeConfig>> runtimeConfigs = new();

        public virtual void GenerateRuntimeConfigs(Func<TConfig, TRuntimeConfig> configConverter)
        {
            runtimeConfigs.Clear();

            if (mode is IntGeneralMultipleConfigMode.Array)
            {
                for (int i = 0; i < arrayConfigs.Count; i++)
                {
                    var runtimeConfig = configConverter(arrayConfigs[i]);
                    runtimeConfigs.Add(i, new List<TRuntimeConfig> { runtimeConfig });
                }
            }
            else if (mode is IntGeneralMultipleConfigMode.Dictionary)
            {
                foreach (var dictionaryConfig in dictionaryConfigs)
                {
                    IEnumerable<int> intValues =
                        dictionaryConfig.useRange ? dictionaryConfig.range : dictionaryConfig.intValues;
                    foreach (var intValue in intValues)
                    {
                        var runtimeConfigsList = runtimeConfigs.GetOrCreate(intValue);

                        foreach (var config in dictionaryConfig.configs)
                        {
                            var runtimeConfig = configConverter(config);
                            runtimeConfigsList.Add(runtimeConfig);
                        }
                    }
                }
            }
        }

        public IReadOnlyList<TRuntimeConfig> GetConfigs(int index)
        {
            if (runtimeConfigs.TryGetValue(index, out var configs))
            {
                return configs;
            }

            return Array.Empty<TRuntimeConfig>();
        }
    }
}