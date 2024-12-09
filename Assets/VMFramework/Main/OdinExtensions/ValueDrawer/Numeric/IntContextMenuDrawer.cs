#if UNITY_EDITOR && ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Core.Editor;

namespace VMFramework.OdinExtensions
{
    [DrawerPriority(DrawerPriorityLevel.SuperPriority)]
    internal sealed class IntContextMenuDrawer : OdinValueDrawer<int>, IDefinesGenericMenuItems
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            CallNextDrawer(label);
        }

        void IDefinesGenericMenuItems.PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            genericMenu.AddSeparator();

            if (ValueEntry.SmartValue != 0)
            {
                genericMenu.AddItem($"Set to zero", () =>
                {
                    ValueEntry.SmartValue = 0;
                });
            }
            
            genericMenu.AddItem($"Set to {int.MaxValue}", () =>
            {
                ValueEntry.SmartValue = int.MaxValue;
            });
            
            genericMenu.AddItem($"Set to {int.MinValue}", () =>
            {
                ValueEntry.SmartValue = int.MinValue;
            });
        }
    }
}

#endif