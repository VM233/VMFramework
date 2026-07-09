#if UNITY_EDITOR && ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Core.Editor;
using VMFramework.Editor;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.OdinExtensions
{
    internal sealed class GamePrefabIDContextMenuDrawer : OdinValueDrawer<string>, IDefinesGenericMenuItems
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            CallNextDrawer(label);
        }

        void IDefinesGenericMenuItems.PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            var value = Property.ValueEntry.WeakSmartValue;

            if (value is not string id)
            {
                return;
            }

            if (GamePrefabWrapperQueryTools.TryGetGamePrefabWrapper(id, out var wrapper) == false)
            {
                return;
            }
            
            genericMenu.AddSeparator();
            
            genericMenu.AddItem(EditorNames.SELECT_ASSET_PATH, wrapper.SelectObject);
            genericMenu.AddItem(EditorNames.OPEN_ASSET_IN_NEW_INSPECTOR_PATH, wrapper.OpenInNewInspector);
            genericMenu.AddItem(EditorNames.OPEN_GAME_PREFAB_SCRIPT_PATH, wrapper.OpenGamePrefabScripts);
            genericMenu.AddItem(EditorNames.OPEN_GAME_ITEM_SCRIPT_PATH, wrapper.OpenGameItemScripts);
        }
    }
}
#endif