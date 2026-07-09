namespace VMFramework.Core
{
    public interface IValidCheckComponent
    {
        public bool IsValid(IValidCheckDispatcher dispatcher);
    }
}