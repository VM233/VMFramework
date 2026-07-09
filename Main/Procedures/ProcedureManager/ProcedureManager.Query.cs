using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core;
using VMFramework.Core.FSM;

namespace VMFramework.Procedure
{
    public partial class ProcedureManager
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasProcedure(string procedureID)
        {
            return procedures.ContainsKey(procedureID);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasCurrentProcedure(string procedureID)
        {
            return fsm.HasCurrentState(procedureID);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IProcedure GetProcedure(string procedureID)
        {
            return procedures.GetValueOrDefault(procedureID);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IProcedure GetProcedureStrictly(string procedureID)
        {
            if (procedures.TryGetValue(procedureID, out var procedure) == false)
            {
                throw new ArgumentException($"Procedure with ID:{procedureID} does not exist.");
            }

            return procedure;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetProcedure(string procedureID, out IProcedure procedure)
        {
            return procedures.TryGetValue(procedureID, out procedure);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetProcedureWithWarning(string procedureID, out IProcedure procedure)
        {
            if (procedures.TryGetValue(procedureID, out procedure) == false)
            {
                UnityEngine.Debug.LogWarning($"Procedure with ID:{procedureID} does not exist.");
                return false;
            }
            
            return true;
        }
    }
}