namespace VMFramework.Core
{
    public readonly struct CircularSelectItem<T> : ICircularSelectItem<T>
    {
        public readonly T value;

        public readonly int times;
        
        public CircularSelectItem(T value, int times = 1)
        {
            this.value = value;
            this.times = times;
        }

        T ICircularSelectItem<T>.Value => value;

        int ICircularSelectItem<T>.Times => times;
    }
}