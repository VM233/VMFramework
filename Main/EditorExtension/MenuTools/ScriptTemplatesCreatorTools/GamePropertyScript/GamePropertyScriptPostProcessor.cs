#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public class GamePropertyScriptPostProcessor : ScriptCreationPostProcessor<GamePropertyScriptExtraInfo>
    {
        protected override void PostProcess(string scriptAbsolutePath, ref string scriptContent,
            GamePropertyScriptExtraInfo extraInfo)
        {
            Replace(ref scriptContent, "GAME_PROPERTY_ID", extraInfo.GamePropertyID);
            Replace(ref scriptContent, "TARGET_TYPE", extraInfo.TargetType);
            Replace(ref scriptContent, "TARGET_NAME", extraInfo.TargetName);
        }
    }
}
#endif