using System;

namespace VMFramework.Procedure
{
    [UnityEngine.Scripting.RequireAttributeUsages]
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class GameInitializerRegister : Attribute
    {
        public readonly string ProcedureID;
        public readonly ProcedureLoadingType LoadingType;

        public GameInitializerRegister(string procedureID, ProcedureLoadingType loadingType)
        {
            ProcedureID = procedureID;
            LoadingType = loadingType;
        }
    }
}
