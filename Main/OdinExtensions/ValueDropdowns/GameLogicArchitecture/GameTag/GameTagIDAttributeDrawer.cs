#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;

namespace VMFramework.OdinExtensions
{
    internal sealed class GameTagIDAttributeDrawer : GeneralValueDropdownAttributeDrawer<GameTagIDAttribute>
    {
        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            return GameTagNameEditorUtility.GetAllGameTagsIDList();
        }
        
        protected override void DrawCustomButtons()
        {
            base.DrawCustomButtons();

            if (Button(GameEditorNames.JUMP_TO_GAME_EDITOR, SdfIconType.Search))
            {
                var gameEditor = EditorWindow.GetWindow<GameEditor>();

                gameEditor.SelectValue<GameTagGeneralSetting>();
            }

            var gameTag = (string)Property.ValueEntry.WeakSmartValue;
            var hasSingleTag = string.IsNullOrWhiteSpace(gameTag) == false &&
                               Property.ValueEntry.ValueState != PropertyValueState.PrimitiveValueConflict;
            using (new EditorGUI.DisabledScope(hasSingleTag == false))
            {
                if (Button("Filter Game Prefabs by this Tag", SdfIconType.Funnel))
                {
                    // The field can belong to the window whose menu tree will be replaced.
                    Property.Tree.DelayAction(() =>
                        EditorWindow.GetWindow<GameEditor>().FilterByGameTag(gameTag));
                }
            }
        }
    }
}
#endif
