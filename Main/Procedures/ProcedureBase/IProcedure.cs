using VMFramework.Core.FSM;

namespace VMFramework.Procedure
{
    [UnityEngine.Scripting.RequireImplementors]
    public interface IProcedure : IMultiFSMState<string, ProcedureManager>
    {
        
    }
}
