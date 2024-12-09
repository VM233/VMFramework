using VMFramework.Containers;

namespace VMFramework.Configuration
{
    public interface IItemGenerationConfig : IConfig
    {
        public IContainerItem GenerateItem();
    }
}