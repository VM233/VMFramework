namespace VMFramework.Core
{
    public interface IDestructible : IReadOnlyDestructible
    {
        public void Destruct();
    }
}