#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Configuration;

namespace VMFramework.UI
{
    public partial class DictionaryConfigPriorityConfig
    {
        public sealed override IEnumerable<ValueDropdownItem> GetPriorityNameList()
        {
            return GetDictionaryConfigs()?.GetNameList();
        }
    }
}
#endif