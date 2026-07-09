namespace VMFramework.Configuration
{
    public interface IGameTagBasedConfigs<TConfig> : IDictionaryConfigs<string, TConfig>
        where TConfig : IConfig
    {
        
    }
}