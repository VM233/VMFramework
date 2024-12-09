namespace VMFramework.Core
{
    public readonly struct WeightedSelectItem<T> : IWeightedSelectItem<T>
    {
        public readonly T value;
        public readonly float weight;
        
        public WeightedSelectItem(T value, float weight)
        {
            this.value = value;
            this.weight = weight;
        }

        T IWeightedSelectItem<T>.Value => value;

        float IWeightedSelectItem<T>.Weight => weight;
    }
}