namespace VMFramework.Properties
{
    public delegate void TimerEndHandler(object owner);
    
    public interface IReadOnlyTimerProperty<out TValue> : IReadOnlyProperty<TValue>
    {
        public bool IsTimerActive { get; }
        
        public event TimerEndHandler OnEnd;

        public float GetScale();
    }
}