namespace VMFramework.Core
{
    public interface IDirtyable
    {
        public delegate void DirtyHandler(IDirtyable dirtyable, bool local);
        
        public event DirtyHandler OnDirty;
    }
}