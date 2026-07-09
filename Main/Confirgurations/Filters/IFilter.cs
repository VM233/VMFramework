namespace VMFramework.Configuration
{
    public interface IFilter
    {
        public bool IsMatch(object obj);
    }
}