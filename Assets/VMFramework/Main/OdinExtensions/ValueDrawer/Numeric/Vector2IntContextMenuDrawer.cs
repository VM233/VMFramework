#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMFramework.OdinExtensions
{
    [DrawerPriority(DrawerPriorityLevel.SuperPriority)]
    internal sealed class Vector2IntContextMenuDrawer : OdinValueDrawer<Vector2Int>, IDefinesGenericMenuItems
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            CallNextDrawer(label);
        }

        void IDefinesGenericMenuItems.PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            genericMenu.AddSeparator();
            
            genericMenu.AddItem($"Set to {CommonVector2Int.maxValue}", () =>
            {
                ValueEntry.SmartValue = CommonVector2Int.maxValue;
            });
            
            genericMenu.AddItem($"Set to {CommonVector2Int.minValue}", () =>
            {
                ValueEntry.SmartValue = CommonVector2Int.minValue;
            });
        }
    }
}
#endif