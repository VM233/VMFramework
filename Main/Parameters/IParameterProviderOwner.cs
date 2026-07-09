namespace VMFramework.Parameters
{
    public interface IParameterProviderOwner
    {
        public IParameterProvider ParameterProvider { get; }
    }
}