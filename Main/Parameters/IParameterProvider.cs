namespace VMFramework.Parameters
{
    public interface IParameterProvider
    {
        public bool TryGetParameter<TValue>(string key, ref TValue value);
    }
}