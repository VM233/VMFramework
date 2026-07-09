using System;
using System.Collections.Generic;

namespace VMFramework.Procedure
{
    public interface IProcedureManager
    {
        public IReadOnlyList<string> CurrentProcedureIDs { get; }
        
        public event Action<string> OnEnterProcedureEvent;
        
        public event Action<string> OnExitProcedureEvent;

        public void EnterProcedure(string fromProcedureID, string toProcedureID);

        public void EnterProcedure(string procedureID);

        public void ExitProcedureImmediately(string procedureID);

        public bool HasProcedure(string procedureID);
        
        public bool HasCurrentProcedure(string procedureID);

        public IProcedure GetProcedure(string procedureID);

        public IProcedure GetProcedureStrictly(string procedureID);

        public bool TryGetProcedure(string procedureID, out IProcedure procedure);

        public bool TryGetProcedureWithWarning(string procedureID, out IProcedure procedure);
    }
}