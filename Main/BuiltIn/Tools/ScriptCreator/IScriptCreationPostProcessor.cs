#if UNITY_EDITOR
namespace VMFramework.Tools.Editor
{
    public interface IScriptCreationPostProcessor
    {
        public void PostProcess(string scriptAbsolutePath, ref string scriptContent, ScriptCreationExtraInfo extraInfo);
    }
}
#endif