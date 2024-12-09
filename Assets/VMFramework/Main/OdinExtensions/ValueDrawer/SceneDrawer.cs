#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VMFramework.OdinExtensions
{
    internal sealed class SceneDrawer : OdinValueDrawer<Scene>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var rect = SirenixEditorGUI.BeginVerticalPropertyLayout(label, out var labelRect);
            var value = this.ValueEntry.SmartValue;
            SirenixEditorFields.IntField("Handle", value.handle);
            EditorGUILayout.TextField("Name", value.name, EditorStyles.textField);
            SirenixEditorGUI.EndVerticalPropertyLayout();
        }
    }
}
#endif