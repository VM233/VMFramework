#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.Localization;
using UnityEditor.Localization.UI;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;
using VMFramework.Editor;
using VMFramework.Localization;

namespace VMFramework.OdinExtensions
{
    internal sealed class TableNameAttributeDrawer : GeneralValueDropdownAttributeDrawer<TableNameAttribute>, 
        IDefinesGenericMenuItems
    {
        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            return LocalizationNameListUtility.GetTableNameList();
        }

        protected override Texture GetSelectorIcon(object value)
        {
            if (value is not string tableName)
            {
                return null;
            }

            if (StringTableCollectionUtility.TryGetStringTableCollection(tableName, out var collection) == false)
            {
                return null;
            }

            return GUIHelper.GetAssetThumbnail(collection, collection.GetType(), true);
        }

        protected override void DrawCustomButtons()
        {
            base.DrawCustomButtons();
            
            if (Button(EditorNames.CREATE_NEW_TABLE, SdfIconType.Plus))
            {
                LocalizationTablesWindow.ShowTableCreator();
            }
            
            var value = Property.ValueEntry.WeakSmartValue;

            if (value is not string tableName)
            {
                return;
            }
            
            StringTableCollection collection = null;

            if (tableName.IsNullOrEmpty() == false)
            {
                collection = LocalizationEditorSettings.GetStringTableCollection(tableName);
            }
            
            if (Button(EditorNames.EDIT_TABLE, SdfIconType.PencilSquare))
            {
                if (collection == null)
                {
                    LocalizationTablesWindow.ShowTableCreator();
                }
                else
                {
                    collection.ShowTable();
                }
            }
            
            if (collection != null)
            {
                if (Button(EditorNames.SELECT_TABLE_ASSET, SdfIconType.Search))
                {
                    collection.SelectObject();
                }
            }
        }

        void IDefinesGenericMenuItems.PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            var value = Property.ValueEntry.WeakSmartValue;

            if (value is not string tableName)
            {
                return;
            }

            if (tableName.IsNullOrWhiteSpace())
            {
                ShowWindow(null);
                return;
            }
            
            var stringTableCollection = LocalizationEditorSettings.GetStringTableCollection(tableName);

            ShowWindow(stringTableCollection);
            
            return;

            void ShowWindow(LocalizationTableCollection collection)
            {
                genericMenu.AddSeparator();
                
                if (collection != null)
                {
                    genericMenu.AddItem(EditorNames.OPEN_TABLE, collection.ShowTable);
                }
                
                genericMenu.AddItem(EditorNames.CREATE_NEW_TABLE, LocalizationTablesWindow.ShowTableCreator);
            }
        }
    }
}
#endif