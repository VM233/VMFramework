using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Procedure;

namespace VMFramework.Timers
{
    [ManagerCreationProvider(ManagerType.TimerCore)]
    [DisallowMultipleComponent]
    public class LogicTickManager : ManagerBehaviour<ILogicTickManager>, ILogicTickManager
    {
        public const int DEFAULT_TICKS_PER_SECOND = 30;
        public const double DEFAULT_TICK_GAP = 1.0 / DEFAULT_TICKS_PER_SECOND;

        public static float CurrentTickDeltaTime =>
            Instance?.TickDeltaTime ?? (float)DEFAULT_TICK_GAP;

        public static float CurrentTickInterpolationAlpha =>
            Instance?.TickInterpolationAlpha ?? 1;

        public bool autoStart = true;
        
        public bool enableTickGapOverride;
        
        [ShowIf(nameof(enableTickGapOverride))]
        public double tickGapOverride = DEFAULT_TICK_GAP;
        
        [ShowInInspector, DisplayAsString]
        public double TickGap { get; private set; }

        [ShowInInspector, DisplayAsString]
        public float TickDeltaTime => (float)TickGap;

        [ShowInInspector, DisplayAsString]
        public float TickInterpolationAlpha =>
            TickGap > 0
                ? Mathf.Clamp01((float)(TimeLeftOver / TickGap))
                : 1;
        
        [ShowInInspector, DisplayAsString]
        public bool IsTicking { get; private set; } = false;
        
        [ShowInInspector, DisplayAsString]
        public ulong Tick { get; private set; } = 0;
        
        [ShowInInspector, DisplayAsString]
        public double TimeLeftOver { get; private set; } = 0;

        public event Action OnPreTick;
        public event Action OnTick;
        public event Action OnPreSimulationTick;
        public event Action OnSimulationTick;
        public event Action OnPostSimulationTick;
        public event Action OnPostTick;

        public event Action OnNextTick
        {
            add => nextTickActions.Add(value);
            remove => nextTickActions.Remove(value);
        }
        
        protected readonly HashSet<Action> nextTickActions = new();
        protected readonly List<Action> nextTickActionsTemp = new();

        protected override void Awake()
        {
            base.Awake();

            IsTicking = false;
            Tick = 0;
            TimeLeftOver = 0;
            SetTickGap(enableTickGapOverride
                ? tickGapOverride
                : DEFAULT_TICK_GAP);
            nextTickActions.Clear();
            nextTickActionsTemp.Clear();
        }

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            if (autoStart)
            {
                StartTick();
            }
        }

        public void IncreaseTick()
        {
            Tick++;
            
            OnPreTick?.Invoke();
            
            OnTick?.Invoke();
            
            if (nextTickActions.Count > 0)
            {
                nextTickActionsTemp.Clear();
                nextTickActionsTemp.AddRange(nextTickActions);
                nextTickActions.Clear();

                foreach (var action in nextTickActionsTemp)
                {
                    action.Invoke();
                }
            }

            OnPreSimulationTick?.Invoke();

            OnSimulationTick?.Invoke();

            OnPostSimulationTick?.Invoke();
            
            OnPostTick?.Invoke();
        }

        protected virtual void Update()
        {
            if (IsTicking == false)
            {
                return;
            }

            AdvanceTime(Time.deltaTime);
        }

        public int AdvanceTime(double deltaTime)
        {
            if (double.IsNaN(deltaTime) ||
                double.IsInfinity(deltaTime) ||
                deltaTime < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(deltaTime), deltaTime,
                    "Logic tick elapsed time must be finite and non-negative.");
            }

            if (IsTicking == false || deltaTime == 0)
            {
                return 0;
            }

            TimeLeftOver += deltaTime;
            var advancedTickCount = 0;
            while (true)
            {
                // A tick callback may deliberately change the cadence. The elapsed time paid by
                // the current tick must remain the gap that admitted it; the new gap applies to
                // the next scheduler decision.
                var admittedTickGap = TickGap;
                if (TimeLeftOver < admittedTickGap)
                {
                    break;
                }

                IncreaseTick();
                TimeLeftOver -= admittedTickGap;
                advancedTickCount++;
            }

            return advancedTickCount;
        }
        
        public void SetTickGap(double tickGap)
        {
            if (double.IsNaN(tickGap) ||
                double.IsInfinity(tickGap) ||
                tickGap <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(tickGap), tickGap,
                    "Logic tick gap must be finite and greater than zero.");
            }

            TickGap = tickGap;
        }

        [Button]
        public void StartTick()
        {
            IsTicking = true;
        }
        
        [Button]
        public void StopTick()
        {
            IsTicking = false;
        }
    }
}
