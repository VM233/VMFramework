#if UNITY_EDITOR && ODIN_INSPECTOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.OdinExtensions
{
    public abstract class GamePrefabIDAttributeDrawer<TAttribute> : GeneralValueDropdownAttributeDrawer<TAttribute>
        where TAttribute : GamePrefabIDAttribute
    {
        private readonly List<IGamePrefabIDFilterAttribute> filters = new();
        private HashSet<IGamePrefab> resultGamePrefabs;
        private HashSet<IGamePrefab> filteredGamePrefabs;
        private List<ValueDropdownItem> dropdownItems;
        
        protected override void Initialize()
        {
            base.Initialize();

            filters.Clear();
            foreach (var property in Property.TraverseToRoot(true, property => property.Parent))
            {
                foreach (var attribute in property.Attributes)
                {
                    if (attribute is IGamePrefabIDFilterAttribute filterAttribute)
                    {
                        filters.Add(filterAttribute);
                    }
                }
            }
        }

        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            if (filters.Count == 0)
            {
                filters.Add(Attribute);
            }
            
            resultGamePrefabs ??= new();
            filteredGamePrefabs??= new();
            
            resultGamePrefabs.Clear();
            filteredGamePrefabs.Clear();
            
            resultGamePrefabs.UnionWith(filters[0].GetGamePrefabs());

            for (int i = 1; i < filters.Count; i++)
            {
                var filter = filters[i];
                
                filteredGamePrefabs.UnionWith(filter.GetGamePrefabs());
                
                resultGamePrefabs.IntersectWith(filteredGamePrefabs);
                filteredGamePrefabs.Clear();
            }

            dropdownItems ??= new();
            dropdownItems.Clear();
            
            foreach (var gamePrefab in resultGamePrefabs)
            {
                dropdownItems.Add(gamePrefab.GetNameIDDropDownItem());
            }
            
            return dropdownItems;
        }

        protected override Texture GetSelectorIcon(object value)
        {
            if (value is not string id)
            {
                return null;
            }

            if (GamePrefabManager.TryGetGamePrefab(id, out var prefab) == false)
            {
                return null;
            }

            if (prefab is not IGameEditorMenuTreeNode node)
            {
                return null;
            }

            return node.Icon.GetTexture();
        }

        protected override void DrawCustomButtons()
        {
            base.DrawCustomButtons();
            
            if (Property.ValueEntry.WeakSmartValue is not string id)
            {
                return;
            }

            if (Button(EditorNames.OPEN_ASSET_IN_NEW_INSPECTOR, SdfIconType.PencilSquare))
            {
                if (GamePrefabWrapperQueryTools.TryGetGamePrefabWrapper(id, out var wrapper))
                {
                    GUIHelper.OpenInspectorWindow(wrapper);
                }
            }

            if (Button(GameEditorNames.JUMP_TO_GAME_EDITOR, SdfIconType.Search))
            {
                if (GamePrefabWrapperQueryTools.TryGetGamePrefabWrapper(id, out var wrapper))
                {
                    var gameEditor = EditorWindow.GetWindow<GameEditor>();
                
                    gameEditor.SelectValue(wrapper);
                }
            }
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (isList == false)
            {
                if (Property.ValueEntry.TypeOfValue != typeof(string))
                {
                    CallNextDrawer(label);
                    return;
                }
            }
            else
            {
                if (Property.ChildResolver is ICollectionResolver collectionResolver)
                {
                    if (collectionResolver.ElementType != typeof(string))
                    {
                        CallNextDrawer(label);
                        return;
                    }
                }
            }
            
            base.DrawPropertyLayout(label);
        }
    }

    internal sealed class GamePrefabIDAttributeDrawer : GamePrefabIDAttributeDrawer<GamePrefabIDAttribute>
    {

    }
}
#endif