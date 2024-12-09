#if UNITY_EDITOR && ODIN_INSPECTOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.OdinExtensions
{
    public abstract class GamePrefabIDAttributeDrawer<TAttribute> : GeneralValueDropdownAttributeDrawer<TAttribute>
        where TAttribute : GamePrefabIDAttribute
    {
        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            if (Attribute.FilterByGameItemType)
            {
                return GamePrefabNameListQuery.GetGamePrefabNameListByGameItemType(Attribute.GameItemType);
            }

            return GamePrefabNameListQuery.GetGamePrefabNameListByType(Attribute.GamePrefabTypes);
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
    }

    internal sealed class GamePrefabIDAttributeDrawer : GamePrefabIDAttributeDrawer<GamePrefabIDAttribute>
    {

    }
}
#endif