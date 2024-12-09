using System.Collections.Generic;

namespace VMFramework.Core.FSM
{
    public interface IMultiFSM<TID, TOwner> : IFSM<TID, TOwner>
    {
        public IReadOnlyDictionary<TID, IMultiFSMState<TID, TOwner>> States { get; }

        public IReadOnlyDictionary<TID, IMultiFSMState<TID, TOwner>> CurrentStates { get; }

        public void Init(TOwner owner);

        public void AddState(IMultiFSMState<TID, TOwner> fsmState);
        
        public bool CanExitState(TID stateID);
        
        public bool ExitState(TID stateID);
    }
}