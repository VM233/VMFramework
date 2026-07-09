namespace VMFramework.GameEvents
{
    public interface IReadOnlyPriorityEvents<in TDelegate>
    {
        public void Add(int priority, TDelegate callback);
        
        public void Remove(TDelegate callback);
    }
}