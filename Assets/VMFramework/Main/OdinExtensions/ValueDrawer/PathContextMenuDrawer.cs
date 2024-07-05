﻿#if UNITY_EDITOR && ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Core.Editor;

namespace VMFramework.OdinExtensions
{
    internal sealed class PathContextMenuDrawer : OdinValueDrawer<string>, IDefinesGenericMenuItems
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            CallNextDrawer(label);
        }

        void IDefinesGenericMenuItems.PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            var value = property.ValueEntry.WeakSmartValue;

            if (value is not string str)
            {
                return;
            }

            bool isPath = false;
            string path = str.TrimStart('/', '\\');

            if (str.StartsWith("Assets/"))
            {
                isPath = true;
            }
            else if (str.StartsWith("Resources/"))
            {
                path = "Assets/" + path;
                isPath = true;
            }

            if (isPath == false)
            {
                return;
            }
            
            path = path.ConvertAssetPathToAbsolutePath();
            path = path.GetDirectoryPath();

            if (path.ExistsDirectory() == false)
            {
                return;
            }
            
            genericMenu.AddItem("Open in Explorer", () =>
            {
                path.OpenDirectory(false);
            });
        }
    }
}
#endif