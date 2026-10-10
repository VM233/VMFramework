using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace VMFramework.Procedure
{
    public interface IProcedureManager
    {
        public CancellationToken LifetimeToken { get; }
        public bool IsLoading { get; }
        public IReadOnlyList<string> CurrentProcedureIDs { get; }
        public event Action<string> OnEnterProcedureEvent;
        public event Action<string> OnExitProcedureEvent;
        public Task EnterProcedure(string fromProcedureID, string toProcedureID);
        public Task EnterProcedure(string procedureID);
        public Task EnterProcedureImmediately(string procedureID);
        public Task ExitProcedureImmediately(string procedureID);
        public Task RetireAsync();
        ProcedureResourceRetirement RetirementProduct { get; }
        public bool HasProcedure(string procedureID);
        public bool HasCurrentProcedure(string procedureID);
        public IProcedure GetProcedure(string procedureID);
        public IProcedure GetProcedureStrictly(string procedureID);
        public bool TryGetProcedure(string procedureID, out IProcedure procedure);
        public bool TryGetProcedureWithWarning(string procedureID, out IProcedure procedure);
    }
}
