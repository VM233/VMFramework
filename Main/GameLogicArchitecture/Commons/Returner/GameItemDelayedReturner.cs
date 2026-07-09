using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Sirenix.OdinInspector;
using VMFramework.Procedure;
using VMFramework.Timers;

namespace VMFramework.GameLogicArchitecture
{
    [ManagerCreationProvider(ManagerType.GameItemCore)]
    public class GameItemDelayedReturner : ManagerBehaviour<GameItemDelayedReturner>
    {
        public class DelayedTimer : Timer<double>
        {
            public IGameItem GameItem { get; set; }
        }

        protected readonly Stack<DelayedTimer> timersPool = new();

        [ShowInInspector]
        protected readonly Dictionary<IGameItem, DelayedTimer> timersLookup = new();

        protected override void Awake()
        {
            base.Awake();

            timersLookup.Clear();
            timersPool.Clear();
        }

        public void DelayedReturn([DisallowNull] IGameItem gameItem, float delay)
        {
            if (gameItem is IGameItemDelayedReturnReceiver receiver)
            {
                receiver.DelayedReturn();
            }

            if (timersPool.TryPop(out var timer) == false)
            {
                timer = new DelayedTimer();
                timer.OnTimed += OnTimed;
            }

            timer.GameItem = gameItem;

            timersLookup[gameItem] = timer;
            
            TimerManager.Instance.Add(timer, delay);
        }

        public void CancelReturn(IGameItem gameItem)
        {
            if (timersLookup.Remove(gameItem, out var timer) == false)
            {
                return;
            }

            TimerManager.Instance.TryStop(timer);
            
            timersPool.Push(timer);
            timer.GameItem = null;
        }

        protected virtual void OnTimed(Timer<double> timer)
        {
            var delayedTimer = (DelayedTimer)timer;
            GameItemManager.Instance.Return(delayedTimer.GameItem);

            timersPool.Push(delayedTimer);

            timersLookup.Remove(delayedTimer.GameItem);
            delayedTimer.GameItem = null;
        }
    }
}