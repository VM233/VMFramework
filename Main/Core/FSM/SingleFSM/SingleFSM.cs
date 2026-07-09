using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace VMFramework.Core.FSM
{
    [HideDuplicateReferenceBox]
    public sealed class SingleFSM<TID, TOwner> : ISingleFSM<TID, TOwner>
    {
        [ShowInInspector]
        public ISingleFSMState<TID, TOwner> CurrentState { get; private set; }
        
        [ShowInInspector]
        public bool InitDone { get; private set; }
        
        [ShowInInspector]
        public TOwner Owner { get; private set; }

        [ShowInInspector]
        private readonly Dictionary<TID, ISingleFSMState<TID, TOwner>> states = new();

        public IReadOnlyDictionary<TID, ISingleFSMState<TID, TOwner>> States => states;

        public void Init(TOwner owner, TID initialStateID)
        {
            if (InitDone)
            {
                throw new InvalidOperationException("FSM already initialized.");
            }

            Owner = owner;

            foreach (var state in states.Values)
            {
                state.Init(this);
            }

            if (states.TryGetValue(initialStateID, out var initialState) == false)
            {
                throw new KeyNotFoundException($"initialStateID: {initialStateID} not found.");
            }
            
            CurrentState = initialState;

            InitDone = true;
        }

        public void Update()
        {
            foreach (var state in states.Values)
            {
                if (state.id.Equals(CurrentState.id))
                {
                    state.Update(true);
                }
                else
                {
                    state.Update(false);
                }
            }
        }

        public void FixedUpdate()
        {
            foreach (var state in states.Values)
            {
                if (state.id.Equals(CurrentState.id))
                {
                    state.FixedUpdate(true);
                }
                else
                {
                    state.FixedUpdate(false);
                }
            }
        }

        public void AddState(ISingleFSMState<TID, TOwner> fsmState)
        {
            if (InitDone)
            {
                throw new InvalidOperationException("FSM已经初始化");
            }

            if (states.TryAdd(fsmState.id, fsmState) == false)
            {
                throw new ArgumentException($"Duplicate state ID: {fsmState.id}");
            }
        }
        
        public bool CanEnterState(TID stateID)
        {
            if (CurrentState.id.Equals(stateID))
            {
                return true;
            }

            if (this.TryGetState(stateID, out var state) == false)
            {
                throw new KeyNotFoundException($"State ID: {stateID} not found.");
            }

            if (CurrentState.CanExitTo(state) == false)
            {
                return false;
            }
            
            return state.CanEnterFrom(CurrentState);
        }

        public bool EnterState(TID stateID)
        {
            if (CurrentState.id.Equals(stateID))
            {
                return true;
            }

            if (this.TryGetState(stateID, out var state) == false)
            {
                throw new KeyNotFoundException($"State ID: {stateID} not found.");
            }

            if (CurrentState.CanExitTo(state) == false)
            {
                return false;
            }

            if (state.CanEnterFrom(CurrentState) == false)
            {
                return false;
            }
            
            var oldState = CurrentState;
            CurrentState = state;
            
            oldState.OnExitTo(CurrentState);
            CurrentState.OnEnterFrom(oldState);
            
            return true;
        }
    }
}