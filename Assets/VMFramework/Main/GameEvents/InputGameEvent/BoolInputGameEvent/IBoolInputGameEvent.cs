namespace VMFramework.GameEvents
{
    public interface IBoolInputGameEvent : IInputGameEvent<bool>
    {
        public bool value { get; }
    }
}