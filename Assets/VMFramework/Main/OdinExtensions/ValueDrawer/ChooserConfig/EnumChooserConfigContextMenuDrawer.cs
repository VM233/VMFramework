#if UNITY_EDITOR && ODIN_INSPECTOR
using System;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core.Editor;

namespace VMFramework.OdinExtensions
{
    internal sealed class EnumChooserConfigContextMenuDrawer<T, TEnum> : OdinValueDrawer<T>, IDefinesGenericMenuItems
        where T : ICollectionWrapperChooserConfig<TEnum, TEnum>
        where TEnum : struct, Enum
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            CallNextDrawer(label);
        }

        void IDefinesGenericMenuItems.PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            var chooser = ValueEntry.SmartValue;

            if (chooser == null)
            {
                return;
            }
            
            genericMenu.AddItem("Add All Enum Values", AddAllEnumValues);
            
            return;

            void AddAllEnumValues()
            {
                foreach (var enumValue in Enum.GetValues(typeof(TEnum)))
                {
                    if (chooser.ContainsWrapper((TEnum)enumValue))
                    {
                        continue;
                    }
                    chooser.AddWrapper((TEnum)enumValue);
                }
            }
        }
    }
}
#endif