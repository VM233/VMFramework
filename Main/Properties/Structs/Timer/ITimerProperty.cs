namespace VMFramework.Properties
{
    public interface ITimerProperty<TValue> : IReadOnlyTimerProperty<TValue>, IProperty<TValue>
    {
        public float Scale { get; set; }

        public void SetScale(float scale);
    }
}