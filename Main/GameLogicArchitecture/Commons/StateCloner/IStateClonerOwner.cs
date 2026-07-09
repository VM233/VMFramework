namespace VMFramework.GameLogicArchitecture
{
    public interface IStateClonerOwner
    {
        public IStateCloner StateCloner { get; }
    }
}