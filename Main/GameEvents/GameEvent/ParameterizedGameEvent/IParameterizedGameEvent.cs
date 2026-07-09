namespace VMFramework.GameEvents
{
    public interface IParameterizedGameEvent<TArgument> : IGameEvent, IReadOnlyParameterizedGameEvent<TArgument>
    {
        public void Propagate(TArgument argument);
    }
}