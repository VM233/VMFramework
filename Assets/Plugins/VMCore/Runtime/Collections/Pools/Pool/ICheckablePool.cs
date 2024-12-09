namespace VMFramework.Core.Pools
{
    public interface ICheckablePool<T> : IPool<T>
    {
        public bool Contains(T item);
    }
}