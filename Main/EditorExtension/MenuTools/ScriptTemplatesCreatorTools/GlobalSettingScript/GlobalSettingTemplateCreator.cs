#if UNITY_EDITOR
using UnityEditor;
using VMFramework.Core;
using VMFramework.Tools.Editor;

namespace VMFramework.Editor
{
    public static class GlobalSettingTemplateCreator
    {
        private static readonly GlobalSettingScriptPostProcessor settingPostProcessor = new();
        private static readonly GlobalSettingFileScriptPostProcessor settingFilePostProcessor = new();

        [MenuItem(UnityMenuItemNames.SCRIPT_TEMPLATE + "Global Setting")]
        private static void CreateGlobalSettingScript()
        {
            ScriptTemplatesCreatorTools.CreateScript<GlobalSettingScriptCreationViewer>(info =>
            {
                var globalSettingFileName = info.ClassName + "File";

                string folderPath = info.folderType.ToFolderString();

                ScriptCreator.CreateScriptAssets(ScriptTemplatesNames.GLOBAL_SETTING, info.ClassName,
                    info.assetFolderPath, extraInfo: new GlobalSettingScriptExtraInfo()
                    {
                        NamespaceName = info.namespaceName,
                        globalSettingFileName = globalSettingFileName
                    }, postProcessor: settingPostProcessor);

                ScriptCreator.CreateScriptAssets(ScriptTemplatesNames.GLOBAL_SETTING_FILE, globalSettingFileName,
                    info.assetFolderPath, extraInfo: new GlobalSettingFileScriptExtraInfo()
                    {
                        NamespaceName = info.namespaceName,
                        folderPath = folderPath,
                        nameInGameEditor = '\"' + info.ClassName.ToPascalCase(" ") + '\"'
                    }, postProcessor: settingFilePostProcessor);
            });
        }
    }
}
#endif