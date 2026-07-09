namespace VMFramework.Core.FSM
{
    public interface IFSM<TID, TOwner>
    {
        public TOwner Owner { get; }
        
        public bool InitDone { get; }
        
        public void Update();

        public void FixedUpdate();
        
        public bool CanEnterState(TID stateID);
        
        public bool EnterState(TID stateID);
    }
}