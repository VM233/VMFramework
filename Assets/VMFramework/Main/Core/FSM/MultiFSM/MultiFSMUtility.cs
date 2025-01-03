﻿using System.Runtime.CompilerServices;

namespace VMFramework.Core.FSM
{
    public static class MultiFSMUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasState<TID, TOwner>(this IMultiFSM<TID, TOwner> fsm, TID stateID)
        {
            return fsm.States.ContainsKey(stateID);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetState<TID, TOwner>(this IMultiFSM<TID, TOwner> fsm, TID stateID,
            out IMultiFSMState<TID, TOwner> state)
        {
            return fsm.States.TryGetValue(stateID, out state);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetState<TID, TOwner, TState>(this IMultiFSM<TID, TOwner> fsm, TID stateID,
            out TState state) where TState : IMultiFSMState<TID, TOwner>
        {
            if (fsm.States.TryGetValue(stateID, out IMultiFSMState<TID, TOwner> stateObj))
            {
                state = (TState) stateObj;
                return true;
            }

            state = default;
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasCurrentState<TID, TOwner>(this IMultiFSM<TID, TOwner> fsm, TID stateID)
        {
            return fsm.CurrentStates.ContainsKey(stateID);
        }
    }
}