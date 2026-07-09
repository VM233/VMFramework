using System;

namespace VMFramework.Timers
{
    public interface ILogicTickManager
    {
        public double TickGap { get; }
        
        public bool IsTicking { get; }
        
        public ulong Tick { get; }
        
        public double TimeLeftOver { get; }
        
        public event Action OnPreTick;
        public event Action OnTick;
        public event Action OnPostTick;
        public event Action OnNextTick;

        public void IncreaseTick();

        public void SetTickGap(float tickGap);

        public void StartTick();

        public void StopTick();
    }
}