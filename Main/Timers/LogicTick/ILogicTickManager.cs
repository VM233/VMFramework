using System;

namespace VMFramework.Timers
{
    public interface ILogicTickManager
    {
        public double TickGap { get; }

        /// <summary>
        /// The immutable duration admitted for the currently executing tick. Outside a tick,
        /// returns the active gap that will admit the next tick.
        /// </summary>
        public float TickDeltaTime { get; }

        public float TickInterpolationAlpha { get; }
        
        public bool IsTicking { get; }
        
        public ulong Tick { get; }
        
        public double TimeLeftOver { get; }
        
        public event Action OnPreTick;
        public event Action OnTick;

        /// <summary>
        /// Runs after logic and next-tick actions have settled, before the simulation owner advances.
        /// Physics command producers should publish their final per-tick state here.
        /// </summary>
        public event Action OnPreSimulationTick;

        /// <summary>
        /// Reserved for the single simulation owner that advances deterministic simulation state.
        /// </summary>
        public event Action OnSimulationTick;

        /// <summary>
        /// Runs after simulation so collision and achieved-motion consumers observe the completed step.
        /// </summary>
        public event Action OnPostSimulationTick;

        public event Action OnPostTick;
        public event Action OnNextTick;

        public void IncreaseTick();

        public int AdvanceTime(double deltaTime);

        public void SetTickGap(double tickGap);

        public void StartTick();

        public void StopTick();
    }
}
