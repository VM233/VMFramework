#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMFramework.OdinExtensions
{
    [DrawerPriority(DrawerPriorityLevel.SuperPriority)]
    internal sealed class Vector3IntContextMenuDrawer : OdinValueDrawer<Vector3Int>, IDefinesGenericMenuItems
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            CallNextDrawer(label);
        }

        void IDefinesGenericMenuItems.PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            genericMenu.AddSeparator();
            
            genericMenu.AddItem($"Set to {CommonVector3Int.maxValue}", () =>
            {
                ValueEntry.SmartValue = CommonVector3Int.maxValue;
            });
            
            genericMenu.AddItem($"Set to {CommonVector3Int.minValue}", () =>
            {
                ValueEntry.SmartValue = CommonVector3Int.minValue;
            });
        }
    }
}
#endif