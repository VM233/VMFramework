namespace VMFramework.Parameters
{
    public interface IParameterSourceConfig
    {
        public IParameterSource GetParameterSource();
    }
    
    public interface IParameterSourceConfig<TType> : IParameterSourceConfig
    {
        public new IParameterSource<TType> GetParameterSource();

        IParameterSource IParameterSourceConfig.GetParameterSource()
        {
            return GetParameterSource();
        }
    }
}