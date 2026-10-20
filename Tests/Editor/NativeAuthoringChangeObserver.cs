using System;
using UnityEditor;

namespace VMFramework.Editor.Tests
{
    internal sealed class NativeAuthoringChangeObserver : AssetPostprocessor
    {
        internal static event Action<string[], string[], string[], string[]> Changed;

        public override int GetPostprocessOrder() => int.MaxValue;

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths) =>
            Changed?.Invoke(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths);
    }
}
