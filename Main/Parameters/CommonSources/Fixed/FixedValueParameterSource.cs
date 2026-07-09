namespace VMFramework.Parameters
{
    public struct FixedValueParameterSource<TValue> : IParameterSource<TValue>
    {
        public TValue fixedValue;

        public FixedValueParameterSource(TValue fixedValue)
        {
            this.fixedValue = fixedValue;
        }

        public bool TryGetValue(ref TValue value)
        {
            value = fixedValue;
            return true;
        }
    }
}