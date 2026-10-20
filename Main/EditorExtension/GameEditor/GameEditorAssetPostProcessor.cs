#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.Editor.GameEditor
{
    internal sealed class GameEditorAssetPostProcessor : AssetPostprocessor
    {
        private static readonly HashSet<GameEditor> pending = new();

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            AssemblyReloadEvents.beforeAssemblyReload += Cancel;
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.ExitingEditMode) Cancel();
            };
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            bool configuredRootImported = ImportsConfiguredRoot(importedAssets.Concat(movedAssets));
            foreach (var editor in Resources.FindObjectsOfTypeAll<GameEditor>())
                if (configuredRootImported ||
                    editor.UsesChangedAssets(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths))
                    pending.Add(editor);
            if (pending.Count == 0) return;
            EditorApplication.delayCall -= Publish;
            EditorApplication.delayCall += Publish;
        }

        private static bool ImportsConfiguredRoot(IEnumerable<string> paths)
        {
            foreach (string path in paths)
            {
                var type = AssetDatabase.GetMainAssetTypeAtPath(path);
                if (type != null && typeof(IGlobalSettingFile).IsAssignableFrom(type) &&
                    GlobalSettingFileManager.TypeConfigs.ContainsKey(type) &&
                    GlobalSettingFileEditorManager.TryGetGlobalSettingPath(type, out var configuredPath, out _) &&
                    configuredPath == path)
                    return true;
            }
            return false;
        }

        private static void Publish()
        {
            var editors = pending.ToArray();
            Cancel();
            foreach (var editor in editors)
                if (editor != null) editor.ForceMenuTreeRebuild();
        }

        private static void Cancel()
        {
            EditorApplication.delayCall -= Publish;
            pending.Clear();
        }
    }
}
#endif
