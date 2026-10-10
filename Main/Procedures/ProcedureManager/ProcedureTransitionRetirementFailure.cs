using System;

namespace VMFramework.Procedure
{
    public sealed class ProcedureTransitionRetirementFailure
    {
        public string ProcedureID { get; }
        public ProcedureTransitionPhase Phase { get; }
        public Exception Exception { get; }
        public ProcedureTransitionRetirementFailure(string procedureID, ProcedureTransitionPhase phase, Exception exception)
        { ProcedureID = procedureID; Phase = phase; Exception = exception; }
    }
}
