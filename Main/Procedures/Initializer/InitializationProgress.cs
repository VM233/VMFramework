namespace VMFramework.Procedure
{
    public readonly struct InitializationProgress
    {
        public int Completed { get; }
        public int Total { get; }

        public InitializationProgress(int completed, int total)
        {
            Completed = completed;
            Total = total;
        }
    }
}
