﻿#if UNITY_EDITOR && ODIN_INSPECTOR
using System;
using VMFramework.Core;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Core.Editor;

namespace VMFramework.OdinExtensions
{
    [DrawerPriority(DrawerPriorityLevel.SuperPriority)]
    internal sealed class OpenScriptContextMenuDrawer<T> : OdinValueDrawer<T>, IDefinesGenericMenuItems
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            CallNextDrawer(label);
        }

        void IDefinesGenericMenuItems.PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            var typeOfValue = property.ValueEntry.TypeOfValue;

            if (property.ValueEntry.WeakSmartValue is Type type)
            {
                typeOfValue = type;
            }
            else if (typeOfValue.IsSystemType())
            {
                return;
            }
            else if (typeOfValue.IsUnityType())
            {
                return;
            }
            
            genericMenu.AddSeparator();

            genericMenu.AddItem("Open Script", () =>
            {
                typeOfValue.OpenScriptOfType();
            });
        }
    }
}

#endif