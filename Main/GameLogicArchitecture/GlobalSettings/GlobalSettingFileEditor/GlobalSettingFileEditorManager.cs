#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.Editor;
using Object = UnityEngine.Object;

namespace VMFramework.GameLogicArchitecture.Editor
{
    public static class GlobalSettingFileEditorManager
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SaveAll()
        {
            foreach (var globalSettingFile in GetGlobalSettings())
            {
                var unityObj = (Object)globalSettingFile;
                
                unityObj.SetEditorDirty();

                foreach (var generalSetting in globalSettingFile.GetAllGeneralSettings())
                {
                    ((Object)generalSetting).SetEditorDirty();
                }
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [MenuItem(UnityMenuItemNames.GLOBAL_SETTINGS + "Check Global Settings File")]
        public static void CheckGlobalSettingsFile()
        {
            AssetDatabase.Refresh();
            
            foreach (var (type, path, fileName) in GetGlobalSettingsPaths())
            {
                var asset = type.FindOrCreateScriptableObjectAtPath(path, out var isNewlyCreated);

                if (isNewlyCreated)
                {
                    Debug.Log($"Created new {type.Name} at path {path}.", asset);
                    
                    var entry = asset.CreateOrMoveEntryToDefaultGroup();
                    
                    entry.SetAddress(fileName.GetFileNameWithoutExtensionFromPath());
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IGlobalSettingFile> GetGlobalSettings()
        {
            foreach (var (type, path, fileName) in GetGlobalSettingsPaths())
            {
                var asset = path.GetAssetByPath(type);

                if (asset is IGlobalSettingFile settingFile)
                {
                    yield return settingFile;
                }
                else
                {
                    Debug.LogWarning($"{nameof(IGlobalSettingFile)} at path {path} is not of type {type}.");
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetGlobalSettingPath(this IGlobalSettingFile globalSettingFile, out string path,
            out string fileName)
        {
            return TryGetGlobalSettingPath(globalSettingFile.GetType(), out path, out fileName);
        }

        public static bool TryGetGlobalSettingPath(Type globalSettingFileType, out string path, out string fileName)
        {
            if (GlobalSettingFileManager.TypeConfigs.TryGetValue(globalSettingFileType, out var config) ==
                false)
            {
                Debug.LogWarning($"Cannot find config for type {globalSettingFileType}.");
                
                path = null;
                fileName = null;
                
                return false;
            }
            
            if (globalSettingFileType.TryGetAttribute(false, out GlobalSettingFileEditorConfigAttribute editorConfig) ==
                false)
            {
                Debug.LogWarning($"Type {globalSettingFileType.Name} does not have a " +
                                 $"{nameof(GlobalSettingFileEditorConfigAttribute)} attribute.");
                
                path = null;
                fileName = null;
                
                return false;
            }

            if (editorConfig.FolderPath.TryGetAssetFolderPathByRelativeFolderPath(false, out var assetFolderPath) ==
                false)
            {
                Debug.LogWarning($"Type {globalSettingFileType.Name}'s {nameof(GlobalSettingFileEditorConfigAttribute)}" +
                                 $"has an invalid {nameof(GlobalSettingFileEditorConfigAttribute.FolderPath)} " +
                                 $"value: {editorConfig.FolderPath}.");
                
                path = null;
                fileName = null;
                
                return false;
            }
                
            fileName = config.FileName;

            if (fileName.EndsWith(".asset") == false)
            {
                fileName += ".asset";
            }
            
            path = assetFolderPath.PathCombine(fileName);

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<(Type type, string path, string fileName)> GetGlobalSettingsPaths()
        {
            foreach (var type in GlobalSettingFileManager.TypeConfigs.Keys)
            {
                if (TryGetGlobalSettingPath(type, out var directoryPath, out var fileName) == false)
                {
                    continue;
                }
                
                yield return (type, directoryPath, fileName);
            }
        }
    }
}
#endif