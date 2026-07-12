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
            double value = Property.ValueEntry.WeakSmartValue switch
            {
                sbyte sbyteValue => sbyteValue,
                byte byteValue => byteValue,
                short shortValue => shortValue,
                ushort ushortValue => ushortValue,
                uint uintValue => uintValue,
                int intValue => intValue,
                long longValue => longValue,
                ulong ulongValue => ulongValue,
                float floatValue => floatValue,
                double doubleValue => doubleValue,
                decimal decimalValue => (double)decimalValue,
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
