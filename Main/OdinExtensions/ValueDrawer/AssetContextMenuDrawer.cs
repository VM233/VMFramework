#if UNITY_EDITOR && ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Core.Editor;
using VMFramework.Editor;

namespace VMFramework.OdinExtensions
{
    internal sealed class AssetContextMenuDrawer : OdinValueDrawer<Object>, IDefinesGenericMenuItems
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            CallNextDrawer(label);
        }

        void IDefinesGenericMenuItems.PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            var value = property.ValueEntry.WeakSmartValue;

            if (value is not Object obj)
            {
                return;
            }

            if (obj.IsAsset() == false)
            {
                return;
            }
            
            genericMenu.AddItem(EditorNames.DELETE_ASSET, obj.DeleteAssetWithDialog);
            genericMenu.AddItem(EditorNames.OPEN_IN_EXPLORER, obj.OpenInExplorer);
        }
    }
}
#endif