namespace VMFramework.GameEvents
{
    public interface IFloatInputGameEvent : IInputGameEvent<float>
    {
        public float value { get; }
    }
}