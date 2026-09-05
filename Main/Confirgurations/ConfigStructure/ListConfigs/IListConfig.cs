using VMFramework.GameLogicArchitecture;

namespace VMFramework.Configuration
{
    public interface IListConfig<TConfig>
        where TConfig : BaseConfig, INameOwner, IListConfig<TConfig>
    {
        public ListConfigs<TConfig> listConfigs { set; }

        public int index { set; }
    }
}
