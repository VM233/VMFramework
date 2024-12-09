namespace VMFramework.Core
{
    public interface IWeightedSelectItem<out T>
    {
        public T Value { get; }
        public float Weight { get; }
    }
}