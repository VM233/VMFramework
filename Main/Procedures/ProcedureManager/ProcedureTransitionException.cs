using System;
using System.Collections.Generic;

namespace VMFramework.Procedure
{
    public sealed class ProcedureTransitionException : Exception
    {
        public string FromID { get; }
        public string ToID { get; }
        public ProcedureTransitionPhase Phase { get; }
        public Exception Primary { get; }
        public IReadOnlyList<ProcedureTransitionRetirementFailure> RetirementFailures { get; }
        public IReadOnlyList<string> CurrentProcedureIDs { get; }
        public ProcedureTransitionException(string fromID, string toID, ProcedureTransitionPhase phase,
            Exception primary, IReadOnlyList<ProcedureTransitionRetirementFailure> failures, IReadOnlyList<string> current)
            : base($"Procedure '{fromID}' -> '{toID}' failed at '{phase}'.", primary)
        { FromID = fromID; ToID = toID; Phase = phase; Primary = primary; RetirementFailures = failures; CurrentProcedureIDs = current; }
    }
}
