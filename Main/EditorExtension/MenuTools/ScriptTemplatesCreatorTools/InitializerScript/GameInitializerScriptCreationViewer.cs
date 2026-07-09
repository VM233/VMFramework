#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector;
using VMFramework.Procedure;

namespace VMFramework.Editor
{
    public sealed class GameInitializerScriptCreationViewer : InitializerScriptCreationViewer
    {
        [EnumToggleButtons]
        public ProcedureLoadingType loadingType = ProcedureLoadingType.OnEnter;

        protected override string NameSuffix => loadingType switch
        {
            ProcedureLoadingType.OnEnter => "Initializer",
            ProcedureLoadingType.OnExit => "Deinitializer",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
#endif