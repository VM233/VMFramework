#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VMFramework.Procedure.Editor;

namespace VMFramework.Editor.GameEditor
{
    [InitializeOnLoad]
    internal static class GameEditorInitializer
    {
        static GameEditorInitializer()
        {
            EditorInitializer.InitializationCompleted += RefreshOpenGameEditors;
        }

        private static void RefreshOpenGameEditors()
        {
            if (Application.isPlaying)
            {
                return;
            }

            foreach (var gameEditor in Resources.FindObjectsOfTypeAll<GameEditor>())
            {
                gameEditor.Repaint();
                gameEditor.ForceMenuTreeRebuild();
            }
        }
    }
}

#endif
