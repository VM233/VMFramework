namespace VMFramework.Configuration
{
    public interface IInitializableConfig
    {
        public bool InitDone { get; }

        public void Init();
    }
}