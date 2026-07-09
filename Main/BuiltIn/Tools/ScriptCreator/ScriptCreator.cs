#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Tools.Editor
{
    public static class ScriptCreator
    {
        public static readonly IScriptCreationPostProcessor defaultPostProcessor = new ScriptCreationPostProcessor();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyList<TextAsset> GetScriptTemplates(string fileName,
            string relativeAssetFolder = ConfigurationPath.INTERNAL_SCRIPT_TEMPLATES_PATH)
        {
            var results = new List<TextAsset>();

            foreach (var textAsset in new AssetPath(relativeAssetFolder, fileName)
                         .FindAssetsByRelativeAssetPath<TextAsset>())
            {
                if (textAsset.name == fileName)
                {
                    results.Prepend(textAsset);
                }
                else if (textAsset.name.StartsWith(fileName + "."))
                {
                    results.Add(textAsset);
                }
            }

            return results;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<(string path, string extraName)> GetScriptTemplatesPaths(string fileName,
            string relativeAssetFolder = ConfigurationPath.INTERNAL_SCRIPT_TEMPLATES_PATH)
        {
            var assets = GetScriptTemplates(fileName, relativeAssetFolder);

            foreach (var asset in assets)
            {
                var dotIndex = asset.name.IndexOf('.');
                var extraName = dotIndex == -1 ? string.Empty : asset.name[(dotIndex + 1)..];
                yield return (asset.GetAssetPath(), extraName);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateScriptAssets(string templateName, string newScriptName, string newScriptFolderPath,
            string templatesRelativePath = ConfigurationPath.INTERNAL_SCRIPT_TEMPLATES_PATH,
            ScriptCreationExtraInfo extraInfo = null, IScriptCreationPostProcessor postProcessor = null)
        {
            if (newScriptName.EndsWith(".cs"))
            {
                newScriptName = newScriptName[..^3];
            }

            if (newScriptName.IsNullOrEmpty())
            {
                throw new ArgumentException("New script name cannot be null or empty.");
            }

            foreach (var (templatePath, extraName) in GetScriptTemplatesPaths(templateName, templatesRelativePath))
            {
                var newName = newScriptName;

                if (extraName.IsNullOrEmpty() == false)
                {
                    newName = $"{newScriptName}.{extraName}";
                }

                CreateScriptAssetWithContent(newScriptFolderPath.PathCombine(newName), File.ReadAllText(templatePath),
                    extraInfo, postProcessor);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateScriptAssetWithContent(string scriptAssetPath, string scriptContent,
            ScriptCreationExtraInfo extraInfo = null, IScriptCreationPostProcessor postProcessor = null)
        {
            scriptAssetPath = scriptAssetPath.MakeAssetPath(".cs");
            var scriptAbsolutePath = CommonFolders.ProjectFolderPath.PathCombine(scriptAssetPath);

            postProcessor ??= defaultPostProcessor;
            postProcessor.PostProcess(scriptAbsolutePath, ref scriptContent, extraInfo);

            var scriptFolder = scriptAbsolutePath.GetDirectoryPath();

            scriptFolder.CreateDirectory();

            File.WriteAllText(scriptAbsolutePath, scriptContent);

            AssetDatabase.ImportAsset(scriptAssetPath);
        }
    }
}
#endif