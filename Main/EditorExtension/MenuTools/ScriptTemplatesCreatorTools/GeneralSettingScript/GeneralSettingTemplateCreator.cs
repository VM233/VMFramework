#if UNITY_EDITOR
using UnityEditor;
using VMFramework.Core;
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public static class GeneralSettingTemplateCreator
    {
        private static readonly GeneralSettingScriptPostProcessor postProcessor = new();

        [MenuItem(UnityMenuItemNames.SCRIPT_TEMPLATE + "General Setting")]
        private static void CreateGeneralSettingScript()
        {
            ScriptTemplatesCreatorTools.CreateScript<GeneralSettingScriptCreationViewer>(info =>
            {
                ScriptCreator.CreateScriptAssets(ScriptTemplatesNames.GENERAL_SETTING, info.ClassName,
                    info.assetFolderPath, extraInfo: new GeneralSettingScriptExtraInfo()
                    {
                        NamespaceName = info.namespaceName,
                        nameInGameEditor = info.name.ToPascalCase(" ")
                    }, postProcessor: postProcessor);
            });
        }
    }
}
#endif