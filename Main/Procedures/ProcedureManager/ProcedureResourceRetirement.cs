using System;
namespace VMFramework.Procedure
{
    public sealed class ProcedureResourceRetirement
    {
        public bool ResourcesReleased { get; }
        public Exception Failure { get; }
        public ProcedureResourceRetirement(bool resourcesReleased, Exception failure)
        { ResourcesReleased = resourcesReleased; Failure = failure; }
    }
}
