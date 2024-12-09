using System.Collections.Generic;

namespace VMFramework.Core.FSM
{
    public interface ISingleFSM<TID, TOwner> : IFSM<TID, TOwner>
    {
        public IReadOnlyDictionary<TID, ISingleFSMState<TID, TOwner>> States { get; }
        
        public ISingleFSMState<TID, TOwner> CurrentState { get; }
        
        public void AddState(ISingleFSMState<TID, TOwner> fsmState);

        public void Init(TOwner owner, TID initialStateID);
    }
}