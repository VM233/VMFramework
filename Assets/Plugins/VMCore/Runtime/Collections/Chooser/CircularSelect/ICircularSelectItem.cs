namespace VMFramework.Core
{
    public interface ICircularSelectItem<out T>
    {
        public T Value { get; }
        
        public int Times { get; }
    }
}