using System;
using VMFramework.Core.FSM;

namespace VMFramework.GameLogicArchitecture
{
    [Obsolete]
    public class MultiFSMStateGameItem<TOwner> : GameItem, IMultiFSMState<string, TOwner>
    {
        protected IMultiFSM<string, TOwner> FSM { get; private set; }

        void IMultiFSMState<string, TOwner>.Init(IMultiFSM<string, TOwner> fsm)
        {
            FSM = fsm;
            OnInit();
        }

        protected virtual void OnInit()
        {
            
        }

        public virtual bool CanEnter() => true;
        
        public virtual bool CanExit() => true;

        protected virtual void OnEnter()
        {
            
        }

        protected virtual void OnExit()
        {
            
        }

        protected virtual void Update(bool isActive)
        {
            
        }

        protected virtual void FixedUpdate(bool isActive)
        {
            
        }

        void IMultiFSMState<string, TOwner>.OnEnter()
        {
            OnEnter();
        }

        void IMultiFSMState<string, TOwner>.OnExit()
        {
            OnExit();
        }

        void IMultiFSMState<string, TOwner>.Update(bool isActive)
        {
            Update(isActive);
        }

        void IMultiFSMState<string, TOwner>.FixedUpdate(bool isActive)
        {
            FixedUpdate(isActive);
        }
    }
}