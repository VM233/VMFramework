namespace VMFramework.GameLogicArchitecture
{
    public struct StateCloneHint
    {
        public bool isNested;

        public override string ToString()
        {
            return $"[{nameof(isNested)}: {isNested}]";
        }
    }
}