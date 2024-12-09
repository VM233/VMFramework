#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMFramework.OdinExtensions
{
    [DrawerPriority(DrawerPriorityLevel.SuperPriority)]
    internal sealed class CubeIntegerContextMenuDrawer : OdinValueDrawer<CubeInteger>, IDefinesGenericMenuItems
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            CallNextDrawer(label);
        }

        void IDefinesGenericMenuItems.PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            genericMenu.AddSeparator();
            
            genericMenu.AddItem("Set to zero CubeInteger", () =>
            {
                ValueEntry.SmartValue = CubeInteger.Zero;
            });
            
            genericMenu.AddItem("Set to max CubeInteger", () =>
            {
                ValueEntry.SmartValue = CubeInteger.Max;
            });
        }
    }
}
#endif