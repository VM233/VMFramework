namespace VMFramework.Parameters
{
    public struct ParameterSource<TType> : IParameterSource<TType>
    {
        public IParameterProvider provider;
        public string parameterName;

        public ParameterSource(IParameterProvider provider, string parameterName)
        {
            this.provider = provider;
            this.parameterName = parameterName;
        }

        public bool TryGetValue(ref TType value)
        {
            return provider.TryGetParameter(parameterName, ref value);
        }
    }
}