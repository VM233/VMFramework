#if UNITY_EDITOR
using VMFramework.Procedure;

namespace VMFramework.Editor
{
    public sealed class GameInitializerScriptPostProcessor
        : InitializerScriptPostProcessor<GameInitializerScriptExtraInfo>
    {
        protected override void PostProcess(string scriptAbsolutePath, ref string scriptContent,
            GameInitializerScriptExtraInfo extraInfo)
        {
            base.PostProcess(scriptAbsolutePath, ref scriptContent, extraInfo);

            string loadingTypeParameter = nameof(ProcedureLoadingType) + "." + extraInfo.loadingType;

            Replace(ref scriptContent, "LOADING_TYPE_PARAMETER", loadingTypeParameter);
        }
    }
}
#endif