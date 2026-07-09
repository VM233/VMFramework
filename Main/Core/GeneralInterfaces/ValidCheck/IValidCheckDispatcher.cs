namespace VMFramework.Core
{
    public interface IValidCheckDispatcher
    {
        public delegate void ValidCheckHandler(IValidCheckDispatcher dispatcher, ref bool isValid);

        public event ValidCheckHandler OnCheckValid;
    }
}