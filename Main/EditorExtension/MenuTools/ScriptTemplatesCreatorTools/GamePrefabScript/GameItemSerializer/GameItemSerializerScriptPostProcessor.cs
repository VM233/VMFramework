#if UNITY_EDITOR
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public class GameItemSerializerScriptPostProcessor : ScriptCreationPostProcessor<GameItemSerializerScriptExtraInfo>
    {
        protected override void PostProcess(string scriptAbsolutePath, ref string scriptContent,
            GameItemSerializerScriptExtraInfo extraInfo)
        {
            Replace(ref scriptContent, "GAME_ITEM_NAME", extraInfo.GameItemName);
            Replace(ref scriptContent, "GAME_ITEM_INTERFACE_NAME", extraInfo.GameItemInterfaceName);
            Replace(ref scriptContent, "GAME_ITEM_FIELD_NAME", extraInfo.GameItemFieldName);
        }
    }
}
#endif