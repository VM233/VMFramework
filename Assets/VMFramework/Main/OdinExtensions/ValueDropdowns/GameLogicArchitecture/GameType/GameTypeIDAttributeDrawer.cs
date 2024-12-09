// #if UNITY_EDITOR
// using System.Collections.Generic;
// using Sirenix.OdinInspector;
// using UnityEditor;
// using VMFramework.Editor.GameEditor;
// using VMFramework.GameLogicArchitecture;
//
// namespace VMFramework.OdinExtensions
// {
//     internal sealed class GameTypeIDAttributeDrawer : GeneralValueDropdownAttributeDrawer<GameTypeIDAttribute>
//     {
//         protected override IEnumerable<ValueDropdownItem> GetValues()
//         {
//             if (Attribute.LeafGameTypesOnly == false)
//             {
//                 return GameTypeNameUtility.GetAllGameTypeNameList();
//             }
//             
//             return GameTypeNameUtility.GetGameTypeNameList();
//         }
//
//         protected override void DrawCustomButtons()
//         {
//             base.DrawCustomButtons();
//
//             if (Button(GameEditorNames.JUMP_TO_GAME_EDITOR, SdfIconType.Search))
//             {
//                 var gameEditor = EditorWindow.GetWindow<GameEditor>();
//
//                 gameEditor.SelectValue<GameTypeGeneralSetting>();
//             }
//         }
//     }
// }
// #endif