#if UNITY_EDITOR
using VMFramework.Procedure;

namespace VMFramework.Editor
{
    public sealed class GameInitializerScriptExtraInfo : InitializerScriptExtraInfo
    {
        public ProcedureLoadingType loadingType { get; init; }
    }
}
#endif