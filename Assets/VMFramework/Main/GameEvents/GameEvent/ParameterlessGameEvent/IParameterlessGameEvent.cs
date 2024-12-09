namespace VMFramework.GameEvents
{
    public interface IParameterlessGameEvent : IReadOnlyParameterlessGameEvent, IGameEvent
    {
        public void Propagate();
    }
}