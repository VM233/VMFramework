#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Timers;

namespace VMFramework.OdinExtensions
{
    public class LogicTickAttributeDrawer : OdinAttributeDrawer<LogicTickAttribute>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            long value = Property.ValueEntry.WeakSmartValue switch
            {
                uint uintValue => uintValue,
                int intValue => intValue,
                long longValue => longValue,
                ulong ulongValue => (long)ulongValue,
                _ => 0
            };
            var duration = value * LogicTickManager.DEFAULT_TICK_GAP;
            var text = duration.ToString("F2") + "s";
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            CallNextDrawer(label);
            GUILayout.EndVertical();
            GUIHelper.PushGUIEnabled(true);

            var rect = EditorGUILayout
                .GetControlRect(hasLabel: false, EditorGUIUtility.singleLineHeight, options: GUILayout.Width(12f))
                .AlignCenter(12f);

            GUILayout.Label(text, SirenixGUIStyles.RightAlignedGreyMiniLabel, GUILayoutOptions.ExpandWidth(false));

            GUIHelper.PopGUIEnabled();
            GUILayout.EndHorizontal();
        }
    }
}
#endif