#if UNITY_EDITOR
using UnityEditor;
using VMFramework.Core;
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public static class GamePropertyTemplateCreator
    {
        private static readonly GamePropertyScriptPostProcessor postProcessor = new();
        
        [MenuItem(UnityMenuItemNames.SCRIPT_TEMPLATE + "Game Property")]
        private static void CreateScript()
        {
            ScriptTemplatesCreatorTools.CreateScript<GamePropertyScriptCreationViewer>(info =>
            {
                ScriptCreator.CreateScriptAssets(ScriptTemplatesNames.GAME_PROPERTY, info.ClassName,
                    info.assetFolderPath, extraInfo: new GamePropertyScriptExtraInfo()
                    {
                        NamespaceName = info.namespaceName,
                        UsingNamespaces = new[] { info.targetType.Namespace },
                        GamePropertyID = info.ClassName.ToSnakeCase(),
                        TargetType = info.targetType.Name,
                        TargetName = info.targetType.Name.TrimStart('I').ToCamelCase()
                    }, postProcessor: postProcessor);
            });
        }
    }
}
#endif