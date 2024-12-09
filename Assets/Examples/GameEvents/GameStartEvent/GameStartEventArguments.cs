namespace VMFramework.Examples
{
    public readonly struct GameStartEventArguments
    {
        public readonly int playerCount;

        public GameStartEventArguments(int playerCount)
        {
            this.playerCount = playerCount;
        }
    }
}