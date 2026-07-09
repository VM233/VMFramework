#if UNITY_EDITOR && ODIN_INSPECTOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace VMFramework.Timers
{
    public partial class LogicTickTimerManager
    {
        [ShowInInspector]
        [EnableGUI]
        private List<ITimer<ulong>> allTimers => queue.ToList();

        [Button]
        private bool ContainsTimer(ITimer<ulong> timer)
        {
            return Contains(timer);
        }
    }
}
#endif