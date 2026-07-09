namespace VMFramework.Timers
{
    public interface ILogicTickTimerManager
    {
        public void Add(ITimer<ulong> timer, uint delay);

        public void Stop(ITimer<ulong> timer);

        public bool Contains(ITimer<ulong> timer);

        public bool TryStop(ITimer<ulong> timer);

        public bool TryStopAndAdd(ITimer<ulong> timer, uint delay);
    }
}