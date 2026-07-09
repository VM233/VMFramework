#if UNITY_EDITOR && ODIN_INSPECTOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Timers
{
    public partial class TimerManager
    {
        [ShowInInspector]
        [EnableGUI]
        private List<ITimer<double>> allTimers => queue.ToList();

        [Button]
        private void AddTimerDebug(float delay = 5)
        {
            var timer = new Timer<double>
            {
                OnTimed = _ => Debug.LogError(233)
            };

            Add(timer, delay);
        }

        [Button]
        private void AddGameItemDebug([GamePrefabID(true, typeof(ITimer<double>))] string id, float delay = 5)
        {
            var gameItem = GameItemManager.Instance.Get(id);

            var timer = (ITimer<double>)gameItem;

            Add(timer, delay);
        }
    }
}
#endif