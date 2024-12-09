using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace VMFramework.Core.FSM
{
    [HideDuplicateReferenceBox]
    public sealed class MultiFSM<TID, TOwner> : IMultiFSM<TID, TOwner>
    {
        [ShowInInspector]
        public bool InitDone { get; private set; }

        [ShowInInspector]
        public TOwner Owner { get; private set; }
        
        [ShowInInspector]
        private readonly Dictionary<TID, IMultiFSMState<TID, TOwner>> states = new();

        [ShowInInspector]
        private readonly Dictionary<TID, IMultiFSMState<TID, TOwner>> currentStates = new();
        
        public IReadOnlyDictionary<TID, IMultiFSMState<TID, TOwner>> States => states;
        public IReadOnlyDictionary<TID, IMultiFSMState<TID, TOwner>> CurrentStates => currentStates;
        
        public void Init(TOwner owner)
        {
            if (InitDone)
            {
                throw new InvalidOperationException("FSM already initialized");
            }

            Owner = owner;

            foreach (var state in states.Values)
            {
                state.Init(this);
            }

            InitDone = true;
        }
        
        public void AddState(IMultiFSMState<TID, TOwner> fsmState)
        {
            if (InitDone)
            {
                throw new InvalidOperationException("FSM already initialized");
            }

            if (states.TryAdd(fsmState.id, fsmState) == false)
            {
                throw new ArgumentException("Duplicate state ID：" + fsmState.id);
            }
        }

        public bool CanEnterState(TID stateID)
        {
            if (currentStates.ContainsKey(stateID))
            {
                return false;
            }

            if (this.TryGetState(stateID, out var state) == false)
            {
                Debugger.LogWarning($"The State with ID: {stateID} does not exist");
                return false;
            }

            if (state.CanEnter() == false)
            {
                return false;
            }
            
            return true;
        }

        public bool CanExitState(TID stateID)
        {
            if (currentStates.ContainsKey(stateID) == false)
            {
                return false;
            }

            if (this.TryGetState(stateID, out var state) == false)
            {
                Debugger.LogWarning($"The State with ID: {stateID} does not exist");
                return false;
            }

            if (state.CanExit() == false)
            {
                return false;
            }
            
            return true;
        }
        
        public bool EnterState(TID stateID)
        {
            if (currentStates.ContainsKey(stateID))
            {
                return false;
            }

            if (this.TryGetState(stateID, out var state) == false)
            {
                throw new KeyNotFoundException($"Stat ID: {stateID} does not exist");
            }

            if (state.CanEnter() == false)
            {
                return false;
            }
            
            currentStates.Add(stateID, state);
            state.OnEnter();
            
            return true;
        }

        public bool ExitState(TID stateID)
        {
            if (currentStates.ContainsKey(stateID) == false)
            {
                return false;
            }

            if (this.TryGetState(stateID, out var state) == false)
            {
                throw new KeyNotFoundException($"Stat ID: {stateID} does not exist");
            }

            if (state.CanExit() == false)
            {
                return false;
            }
            
            currentStates.Remove(stateID);
            state.OnExit();
            
            return true;
        }

        public void Update()
        {
            foreach (var state in states.Values)
            {
                if (currentStates.ContainsKey(state.id))
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
                if (currentStates.ContainsKey(state.id))
                {
                    state.FixedUpdate(true);
                }
                else
                {
                    state.FixedUpdate(false);
                }
            }
        }
    }
}