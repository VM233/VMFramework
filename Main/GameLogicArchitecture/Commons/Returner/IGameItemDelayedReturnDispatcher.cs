namespace VMFramework.GameLogicArchitecture
{
    public interface IGameItemDelayedReturnDispatcher
    {
        public delegate void DelayedPreReturnHandler(IGameItemDelayedReturnDispatcher dispatcher);

        public event DelayedPreReturnHandler OnDelayedPreReturn;
    }
}