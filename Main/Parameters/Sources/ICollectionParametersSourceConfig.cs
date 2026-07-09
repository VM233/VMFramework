namespace VMFramework.Parameters
{
    public interface ICollectionParametersSourceConfig<TType>
    {
        public ICollectionParametersSource<TType> GetCollectionParameterSource();
    }
}