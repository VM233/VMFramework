#if UNITY_EDITOR
using System;
using System.Runtime.CompilerServices;
using VMFramework.Core.Editor;

namespace VMFramework.Editor
{
    internal static class ScriptTemplatesCreatorTools
    {
        public delegate void CreateScriptDelegate<in TViewer>(TViewer viewer)
            where TViewer : IScriptCreationViewer, new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void CreateScript<TViewer>(CreateScriptDelegate<TViewer> onCreating)
            where TViewer : IScriptCreationViewer, new()
        {
            if (SelectionUtility.TryGetSelectedFolderPath(out var selectedAssetFolderPath) == false)
            {
                throw new InvalidOperationException($"Could not create script. No folder selected.");
            }
            
            var viewer = new TViewer
            {
                AssetFolderPath = selectedAssetFolderPath
            };

            viewer.ShowConfirmViewer(info => { onCreating(info); });
        }
    }
}
#endif