using VMFramework.Configuration;
#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
#endif

namespace VMFramework.UI
{
    [System.Serializable]
    public abstract class DictionaryConfigPriorityConfig : PriorityConfig
    {
        protected DictionaryConfigPriorityConfig()
        {
        }

        protected DictionaryConfigPriorityConfig(string presetID) : base(presetID)
        {
        }

        protected DictionaryConfigPriorityConfig(int priority) : base(priority)
        {
        }

        public abstract IDictionaryConfigs<string, PriorityPreset> GetDictionaryConfigs();

        protected sealed override bool TryGetPriorityFromPreset(string presetID, out int priority)
        {
            var configs = GetDictionaryConfigs();

            if (configs == null)
            {
                priority = 0;
                return false;
            }

            if (configs.TryGetConfigRuntime(presetID, out var config))
            {
                priority = config.priority;
                return true;
            }

            priority = 0;
            return false;
        }
#if UNITY_EDITOR
        public sealed override IEnumerable<ValueDropdownItem> GetPriorityNameList()
        {
            return GetDictionaryConfigs()?.GetNameList();
        }
#endif
    }
}
