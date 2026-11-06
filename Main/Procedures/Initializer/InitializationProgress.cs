namespace VMFramework.Procedure
{
    public readonly struct InitializationProgress
    {
        public int Completed { get; }
        public int Total { get; }
        public float? Fraction { get; }

        public InitializationProgress(int completed, int total)
        {
            Completed = completed;
            Total = total;
            Fraction = total == 0 ? null : completed / (float)total;
        }

        public InitializationProgress(float fraction)
        {
            if (!float.IsFinite(fraction) || fraction < 0 || fraction > 1)
                throw new System.ArgumentOutOfRangeException(nameof(fraction));
            Completed = 0;
            Total = 0;
            Fraction = fraction;
        }
    }
}
