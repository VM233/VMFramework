namespace VMFramework.Parameters
{
    public struct DelegateParameterSource<TValue> : IParameterSource<TValue>
    {
        public IParameterSource<TValue>.ValueGetter func;

        public DelegateParameterSource(IParameterSource<TValue>.ValueGetter func)
        {
            this.func = func;
        }

        public bool TryGetValue(ref TValue value)
        {
            return func(ref value);
        }
    }
}