#if UNITY_EDITOR && ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Core.Editor;

namespace VMFramework.OdinExtensions
{
    [DrawerPriority(DrawerPriorityLevel.SuperPriority)]
    internal sealed class FloatContextMenuDrawer : OdinValueDrawer<float>, IDefinesGenericMenuItems
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            CallNextDrawer(label);
        }

        void IDefinesGenericMenuItems.PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            genericMenu.AddSeparator();
            
            genericMenu.AddItem($"Set to {float.MaxValue}", () =>
            {
                ValueEntry.SmartValue = float.MaxValue;
            });
            
            genericMenu.AddItem($"Set to {float.MinValue}", () =>
            {
                ValueEntry.SmartValue = float.MinValue;
            });
        }
    }
}

#endif