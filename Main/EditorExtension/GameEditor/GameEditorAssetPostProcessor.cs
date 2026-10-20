#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

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
            foreach (var editor in Resources.FindObjectsOfTypeAll<GameEditor>())
                if (editor.UsesChangedAssets(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths))
                    pending.Add(editor);
            if (pending.Count == 0) return;
            EditorApplication.delayCall -= Publish;
            EditorApplication.delayCall += Publish;
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
