namespace VMFramework.UI
{
    public interface IProviderWrapper
    {
        public object Source { get; }
        
        public void SetSource(object source);
    }
}