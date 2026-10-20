using System;
using UnityEditor;
using UnityEditor.Callbacks;

namespace VMFramework.Editor.Tests
{
    internal sealed class NativeAuthoringChangeObserver : AssetPostprocessor
    {
        internal static event Action<string[], string[], string[], string[]> Changed;

        [RunAfterAssembly("VMFramework")]
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths) =>
            Changed?.Invoke(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths);
    }
}
