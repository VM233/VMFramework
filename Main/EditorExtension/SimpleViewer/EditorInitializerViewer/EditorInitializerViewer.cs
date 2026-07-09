#if UNITY_EDITOR
using UnityEditor;
using VMFramework.OdinExtensions;

namespace VMFramework.Editor
{
    internal sealed class EditorInitializerViewer : SimpleOdinEditorWindow<EditorInitializerViewerContainer>
    {
        public const string EDITOR_NAME = "Editor Initializer Viewer";
        
        [MenuItem(UnityMenuItemNames.EDITOR_INITIALIZATION + EDITOR_NAME)]
        public static void OpenWindow() => GetSimpleWindow<EditorInitializerViewer>(EDITOR_NAME);
    }
}
#endif