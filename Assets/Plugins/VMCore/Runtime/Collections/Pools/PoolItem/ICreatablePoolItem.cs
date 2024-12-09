namespace VMFramework.Core.Pools
{
    public interface ICreatablePoolItem : IPoolItem
    {
        public void OnCreate();
    }

    public interface ICreatablePoolItem<in T> : IPoolItem
    {
        public void OnCreate(T argument);
    }
}