#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
            return GameTagNameUtility.GetAllGameTagsNameList();
        }
        
        protected override void DrawCustomButtons()
        {
            base.DrawCustomButtons();

            if (Button(GameEditorNames.JUMP_TO_GAME_EDITOR, SdfIconType.Search))
            {
                var gameEditor = EditorWindow.GetWindow<GameEditor>();

                gameEditor.SelectValue<GameTagGeneralSetting>();
            }
        }
    }
}
#endif