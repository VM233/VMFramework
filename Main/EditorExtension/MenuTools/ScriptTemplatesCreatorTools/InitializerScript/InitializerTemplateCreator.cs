#if UNITY_EDITOR
using UnityEditor;
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public static class InitializerTemplateCreator
    {
        private static readonly GameInitializerScriptPostProcessor gameInitializerPostProcessor = new();

        private static readonly EditorInitializerScriptPostProcessor editorInitializerPostProcessor = new();

        [MenuItem(UnityMenuItemNames.SCRIPT_TEMPLATE + "Game Initializer")]
        private static void CreateGameInitializerScript()
        {
            ScriptTemplatesCreatorTools.CreateScript<GameInitializerScriptCreationViewer>(info =>
            {
                ScriptCreator.CreateScriptAssets(ScriptTemplatesNames.GAME_INITIALIZER, info.ClassName,
                    info.assetFolderPath, extraInfo: new GameInitializerScriptExtraInfo()
                    {
                        NamespaceName = info.namespaceName,
                        initializationOrderName = info.initializationOrder.ToString(),
                        loadingType = info.loadingType
                    }, postProcessor: gameInitializerPostProcessor);
            });
        }

        [MenuItem(UnityMenuItemNames.SCRIPT_TEMPLATE + "Editor Initializer")]
        private static void CreateEditorInitializerScript()
        {
            ScriptTemplatesCreatorTools.CreateScript<EditorInitializerScriptCreationViewer>(info =>
            {
                ScriptCreator.CreateScriptAssets(ScriptTemplatesNames.EDITOR_INITIALIZER, info.ClassName,
                    info.assetFolderPath, extraInfo: new InitializerScriptExtraInfo()
                    {
                        NamespaceName = info.namespaceName,
                        initializationOrderName = info.initializationOrder.ToString()
                    }, postProcessor: editorInitializerPostProcessor);
            });
        }
    }
}
#endif