#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace VMFramework.UI
{
    public partial class PriorityConfig
    {
        public abstract IEnumerable<ValueDropdownItem> GetPriorityNameList();
    }
}
#endif