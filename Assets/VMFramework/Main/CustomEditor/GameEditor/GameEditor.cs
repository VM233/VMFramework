﻿#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Editor.GameEditor
{
    internal class GameEditor : OdinMenuEditorWindow
    {
        private readonly AuxiliaryTools auxiliaryTools = new();

        [MenuItem("Tools/" + GameEditorNames.GAME_EDITOR_NAME + " #G")]
        // [Shortcut("Open " + GameEditorNames.GAME_EDITOR_NAME, KeyCode.G, ShortcutModifiers.Shift)]
        private static void OpenWindow()
        {
            GameCoreSettingFile.CheckGlobal();

            var window = CreateWindow<GameEditor>(GameEditorNames.GAME_EDITOR_NAME);
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            if (GameCoreSetting.gameCoreSettingsFile == null)
            {
                return new OdinMenuTree(true);
            }

            GameCoreSetting.gameCoreSettingsFile.AutoFindSetting();

            var gameEditorSetting = GameCoreSetting.gameEditorGeneralSetting;
            
            OdinMenuTree tree = new(true)
            {
                { GameEditorNames.AUXILIARY_TOOLS_CATEGORY, auxiliaryTools, EditorIcons.HamburgerMenu },
                {
                    GameEditorNames.GENERAL_SETTINGS_CATEGORY, GameCoreSetting.gameCoreSettingsFile,
                    SdfIconType.GearFill
                },
                { GameEditorNames.EDITOR_CATEGORY, null, EditorIcons.UnityLogo },
                { GameEditorNames.CORE_CATEGORY, null, EditorIcons.StarPointer },
                { GameEditorNames.RESOURCES_MANAGEMENT_CATEGORY, null, SdfIconType.Boxes },
                { GameEditorNames.BUILT_IN_CATEGORY, null, SdfIconType.Inboxes },
            };

            tree.DefaultMenuStyle.IconSize = 24.00f;
            tree.Config.DrawSearchToolbar = true;

            var generalSettingsPathDict = new Dictionary<GeneralSetting, string>();

            foreach (var generalSetting in GameCoreSetting.GetAllGeneralSettings())
            {
                if (generalSetting is not IGameEditorMenuTreeNode generalSettingNode)
                {
                    continue;
                }

                var folderPath = generalSettingNode.folderPath;
                folderPath = folderPath.Replace("\\", "/");
                folderPath = folderPath.Trim('/');

                var totalPath = string.Empty;

                if (folderPath.IsNullOrEmptyAfterTrim() == false)
                {
                    totalPath += folderPath;
                }

                totalPath += $"/{generalSettingNode.name}";

                tree.Add(totalPath, generalSetting, generalSettingNode.icon);

                generalSettingsPathDict.Add(generalSetting, totalPath);
            }

            foreach (var (generalSetting, totalPath) in generalSettingsPathDict)
            {
                if (generalSetting is IGameEditorMenuTreeNodesProvider
                    {
                        isMenuTreeNodesVisible: true
                    } menuTreeNodeProvider)
                {
                    var path = totalPath;

                    var allNodes = menuTreeNodeProvider.GetAllMenuTreeNodes()?.ToList();

                    if (allNodes == null)
                    {
                        Debug.LogWarning($"{menuTreeNodeProvider}获取的节点列表为Null");
                        continue;
                    }

                    if (menuTreeNodeProvider.autoStackMenuTreeNodes)
                    {
                        if (gameEditorSetting == null || allNodes.Count >
                            gameEditorSetting.autoStackMenuTreeNodesMaxCount)
                        {
                            path += "/具体设置";
                        }
                    }

                    foreach (var menuTreeNode in allNodes)
                    {
                        if (menuTreeNode == null)
                        {
                            continue;
                        }

                        tree.Add($"{path}/{menuTreeNode.name}", menuTreeNode, menuTreeNode.icon);
                    }
                }
            }

            tree.EnumerateTree().ForEach(AddRightClickContextMenu);

            return tree;
        }

        private void AddRightClickContextMenu(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += menuItem =>
            {
                if (menuItem.Value is not IGameEditorContextMenuProvider contextMenuProvider)
                {
                    return;
                }

                if (Event.current.type == EventType.MouseDown && Event.current.button == 1 &&
                    menuItem.Rect.Contains(Event.current.mousePosition))
                {
                    GenericMenu menu = new GenericMenu();

                    foreach (var config in contextMenuProvider.GetMenuItems())
                    {
                        menu.AddItem(new GUIContent(config.name, config.tooltip), false,
                            () => config.onClick?.Invoke());
                    }

                    menu.ShowAsContext();

                    Event.current.Use();
                }
            };
        }

        protected override void OnBeginDrawEditors()
        {
            if (MenuTree?.Selection == null)
            {
                return;
            }

            var selected = MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = MenuTree.Config.SearchToolbarHeight;

            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            
            if (selected is not { Value: not null })
            {
                SirenixEditorGUI.EndHorizontalToolbar();
                return;
            }
            
            GUILayout.Label(selected.Name);

            if (selected.Value is not IGameEditorToolBarProvider toolBarProvider)
            {
                SirenixEditorGUI.EndHorizontalToolbar();
                return;
            }
            
            var tree = new StringPathTree<IGameEditorToolBarProvider.ToolbarButtonConfig>();
                        
            foreach (var buttonConfig in toolBarProvider.GetToolbarButtons())
            {
                tree.Add(buttonConfig.path, buttonConfig);
            }

            foreach (var buttonNode in tree.root.children.Values)
            {
                if (SirenixEditorGUI.ToolbarButton(new GUIContent(buttonNode.pathPart,
                        buttonNode.data.tooltip)))
                {
                    if (buttonNode.children.Count <= 0)
                    {
                        buttonNode.data.onClick?.Invoke();
                    }
                    else
                    {
                        GenericMenu menu = new GenericMenu();

                        Action action = null;
                        foreach (var leaf in buttonNode.GetAllLeaves(true))
                        {
                            menu.AddItem(new GUIContent(leaf.pathPart, leaf.data.tooltip), false, () =>
                            {
                                leaf.data.onClick?.Invoke();
                            });
                            
                            action = leaf.data.onClick;
                        }

                        if (menu.GetItemCount() > 1)
                        {
                            menu.ShowAsContext();
                        }
                        else
                        {
                            action?.Invoke();
                        }
                    }
                }
            }
            
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}
#endif