#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public abstract class InitializerScriptPostProcessor<TExtraInfo>
        : ScriptCreationPostProcessor<TExtraInfo> where TExtraInfo : InitializerScriptExtraInfo
    {
        protected override void PostProcess(string scriptAbsolutePath, ref string scriptContent,
            TExtraInfo extraInfo)
        {
            Replace(ref scriptContent, "INITIALIZATION_ORDER_NAME", extraInfo.initializationOrderName);
        }
    }
}
#endif