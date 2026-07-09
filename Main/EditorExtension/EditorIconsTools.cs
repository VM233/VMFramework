#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace VMFramework.Editor
{
    public static class EditorIconsTools
    {
        [MenuItem(UnityMenuItemNames.VMFRAMEWORK + "Open Editor Icons Overview")]
        private static void OpenEditorIconsOverview()
        {
            EditorIconsOverview.OpenEditorIconsOverview();
        }
    }
}
#endif