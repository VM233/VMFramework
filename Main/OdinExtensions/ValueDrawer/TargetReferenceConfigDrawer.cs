#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core.Editor;

namespace VMFramework.OdinExtensions
{
    internal sealed class TargetReferenceConfigDrawer<TValue>
        : OdinValueDrawer<TargetReferenceConfig<TValue>>, IDefinesGenericMenuItems
        where TValue : class
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            var rect = GUIHelper.GetCurrentLayoutRect();
            var value = DragAndDropUtilities.DragAndDropZone(rect, null, typeof(Object), allowMove: true,
                allowSwap: true, allowSceneObjects: true);

            if (EditorGUI.EndChangeCheck())
            {
                Property.RecordForUndo("Set Target Reference");
            
                bool anyChanged = false;
                if (value is GameObject go)
                {
                    ValueEntry.SmartValue.type = TargetReferenceType.GameObject;
                    ValueEntry.SmartValue.gameObject = go;
                    anyChanged = true;
                }
                else if (value is Component component)
                {
                    ValueEntry.SmartValue.type = TargetReferenceType.Component;
                    ValueEntry.SmartValue.component = component;
                    anyChanged = true;
                }

                if (anyChanged)
                {
                    ValueEntry.ApplyChanges();
                }
            }
            
            CallNextDrawer(label);
        }

        void IDefinesGenericMenuItems.PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            genericMenu.AddSeparator();

            genericMenu.AddItem("Reset", () =>
            {
                ValueEntry.SmartValue.type = TargetReferenceType.None;
                ValueEntry.SmartValue.component = null;
                ValueEntry.SmartValue.gameObject = null;
                ValueEntry.SmartValue.classReference = null;
            });
        }
    }
}
#endif