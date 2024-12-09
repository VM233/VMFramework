using System;

namespace VMFramework.Core
{
    public interface IRandomItemProvider<out TItem> : IRandomItemProvider
    {
        object IRandomItemProvider.GetRandomObjectItem(Random random)
        {
            return GetRandomItem(random);
        }

        public TItem GetRandomItem(Random random);
    }

    public interface IRandomItemProvider
    {
        public object GetRandomObjectItem(Random random);
    }
}