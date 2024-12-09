namespace VMFramework.Core
{
    public interface IIDOwner<out T>
    {
        public T id { get; }
    }
}
