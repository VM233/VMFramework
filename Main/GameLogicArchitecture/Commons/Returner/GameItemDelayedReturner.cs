using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Procedure;
using VMFramework.Timers;

namespace VMFramework.GameLogicArchitecture
{
    [ManagerCreationProvider(ManagerType.GameItemCore)]
    public class GameItemDelayedReturner : ManagerBehaviour<GameItemDelayedReturner>
    {
        private sealed class DelayedTimer : Timer<double>
        {
            internal IGameItem Item;
            internal ITimerManager TimerOwner;
            internal IGameItemManager ReturnOwner;
            internal CancellationTokenRegistration Destruction;
        }

        private readonly Stack<DelayedTimer> timersPool = new();
        private readonly Dictionary<IGameItem, DelayedTimer> timersLookup = new();

        public void DelayedReturn([DisallowNull] IGameItem gameItem, float delay)
        {
            if (gameItem.IsDestroyed)
                throw new InvalidOperationException($"Cannot schedule returned GameItem '{gameItem.id}'.");
            if (delay < 0 || float.IsNaN(delay) || float.IsInfinity(delay))
                throw new ArgumentOutOfRangeException(nameof(delay));
            if (timersLookup.ContainsKey(gameItem)) return;
            if (gameItem is INaturalReturnGameItem natural && natural.TryBeginNaturalReturn()) return;
            if (gameItem is IGameItemDelayedReturnReceiver receiver) receiver.DelayedReturn();
            if (delay == 0)
            {
                GameItemManager.Instance.Return(gameItem);
                return;
            }
            if (!timersPool.TryPop(out var timer))
            {
                timer = new DelayedTimer();
                timer.OnTimed += OnTimed;
            }
            timer.Item = gameItem;
            timer.TimerOwner = TimerManager.Instance;
            timer.ReturnOwner = GameItemManager.Instance;
            timersLookup.Add(gameItem, timer);
            gameItem.OnDestructed += OnItemDestructed;
            if (gameItem is MonoBehaviour behaviour)
                timer.Destruction = behaviour.destroyCancellationToken.Register(() => CancelReturn(gameItem));
            timer.TimerOwner.Add(timer, delay);
        }

        public void CancelReturn(IGameItem gameItem)
        {
            if (!timersLookup.Remove(gameItem, out var timer)) return;
            timer.TimerOwner.TryStop(timer);
            Release(timer);
        }

        private void OnItemDestructed(IReadOnlyDestructible item) => CancelReturn((IGameItem)item);

        private void OnTimed(Timer<double> timer)
        {
            var delayed = (DelayedTimer)timer;
            var item = delayed.Item;
            var returnOwner = delayed.ReturnOwner;
            timersLookup.Remove(item);
            Release(delayed);
            returnOwner.Return(item);
        }

        private void Release(DelayedTimer timer)
        {
            timer.Item.OnDestructed -= OnItemDestructed;
            timer.Destruction.Dispose();
            timer.Destruction = default;
            timer.Item = null;
            timer.TimerOwner = null;
            timer.ReturnOwner = null;
            timersPool.Push(timer);
        }

        protected override void OnDestroy()
        {
            try
            {
                foreach (var timer in timersLookup.Values)
                {
                    timer.TimerOwner.TryStop(timer);
                    Release(timer);
                }
                timersLookup.Clear();
                timersPool.Clear();
            }
            finally { base.OnDestroy(); }
        }
    }
}
