#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace VMFramework.OdinExtensions
{
    internal sealed class PhysicsSceneDrawer : OdinValueDrawer<PhysicsScene>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var str = nameof(PhysicsScene) + ": " + ValueEntry.SmartValue;
            var labelStyle = new GUIStyle(EditorStyles.label);
            
            if (label == null)
            {
                EditorGUILayout.LabelField(str, labelStyle, GUILayoutOptions.MinWidth(0.0f));
            }
            else
            {
                SirenixEditorGUI.GetFeatureRichControlRect(label, out int _, out bool _, out var valueRect);
                GUI.Label(valueRect, str, labelStyle);
            }
        }
    }
}
#endif