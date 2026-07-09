using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VMFramework.Configuration
{
    public partial class DictionaryConfigs<TID, TConfig> : IReadOnlyCollection<KeyValuePair<TID, TConfig>>
    {
        public int Count
        {
            get
            {
                if (InitDone)
                {
                    return configsRuntime.Count;
                }
                
                return configs.Count;
            }
        }

        public IEnumerator<KeyValuePair<TID, TConfig>> GetEnumerator()
        {
            if (InitDone)
            {
                return configsRuntime.GetEnumerator();
            }

            return configs.Select(config => new KeyValuePair<TID, TConfig>(config.id, config))
                .GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}