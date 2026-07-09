using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    [Serializable]
    public class IntGeneralConfig<TConfig> : IntGeneralConfig<TConfig, TConfig>
    {
        public void GenerateRuntimeConfigs()
        {
            GenerateRuntimeConfigs(config => config);
        }
    }
    
    [Serializable]
    public class IntGeneralConfig<TConfig, TRuntimeConfig>
    {
        public IntGeneralConfigMode mode = IntGeneralConfigMode.Array;

        [ShowIf(nameof(mode), IntGeneralConfigMode.Array)]
        [IsNotNullOrEmpty]
        public List<TConfig> arrayConfigs = new();

        protected Dictionary<int, TRuntimeConfig> runtimeConfigs = new();

        public void GenerateRuntimeConfigs(Func<TConfig, TRuntimeConfig> configConverter)
        {
            runtimeConfigs.Clear();

            if (mode is IntGeneralConfigMode.Array)
            {
                for (int i = 0; i < arrayConfigs.Count; i++)
                {
                    var runtimeConfig = configConverter(arrayConfigs[i]);
                    runtimeConfigs.Add(i, runtimeConfig);
                }
            }
        }

        public bool TryGetConfig(int index, out TRuntimeConfig config)
        {
            return runtimeConfigs.TryGetValue(index, out config);
        }
    }
}