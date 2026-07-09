namespace VMFramework.Parameters
{
    public interface IParameterSource
    {
        public bool TryGetObjectValue(ref object value);
    }

    public interface IParameterSource<TType> : IParameterSource
    {
        public delegate bool ValueGetter(ref TType value);

        public bool TryGetValue(ref TType value);

        bool IParameterSource.TryGetObjectValue(ref object value)
        {
            if (this.TryGetValue(out var typedValue))
            {
                value = typedValue;
                return true;
            }

            return false;
        }
    }
}