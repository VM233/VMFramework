#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;
using VMFramework.Procedure.Editor;

namespace VMFramework.Editor.GameEditor
{
    internal sealed class GameEditor : OdinMenuEditorWindow
    {
        private static readonly List<IGameEditorContextMenuModifier> contextMenuModifiers = new();
        
        static GameEditor()
        {
            contextMenuModifiers.AddInstancesOfDerivedClasses(false);
        }

        [MenuItem(UnityMenuItemNames.VMFRAMEWORK + GameEditorNames.GAME_EDITOR + " #G", false, 100)]
        private static void OpenWindow()
        {
            var window = CreateWindow<GameEditor>(GameEditorNames.GAME_EDITOR);
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            if (EditorInitializer.isInitialized == false && Application.isPlaying == false)
            {
                var loadingPreviewTree = new OdinMenuTree(true)
                {
                    { "Loading...", new GameEditorLoadingPreview() }
                };
                
                loadingPreviewTree.Selection.Add(loadingPreviewTree.RootMenuItem.ChildMenuItems[0]);
                
                return loadingPreviewTree;
            }

            OdinMenuTree tree = new(true);

            var nodesInfo = new Dictionary<IGameEditorMenuTreeNode, TreeNodePreBuildInfo>();
            
            var nodes = new List<IGameEditorMenuTreeNode>();
            
            var leftNodes = new Queue<IGameEditorMenuTreeNode>();
            
            foreach (var globalSettingFile in GlobalSettingFileEditorManager.GetGlobalSettings())
            {
                if (globalSettingFile is IGameEditorMenuTreeNode node)
                {
                    leftNodes.Enqueue(node);
                    nodes.Add(node);
                    var info = SimplePool<TreeNodePreBuildInfo>.Get();
                    info.node = node;
                    nodesInfo.Add(node, info);
                }
            }

            while (leftNodes.Count > 0)
            {
                var leftNode = leftNodes.Dequeue();

                if (leftNode.IsUnityNull())
                {
                    continue;
                }

                if (leftNode is not IGameEditorMenuTreeNodesProvider provider)
                {
                    continue;
                }

                foreach (var child in provider.GetAllMenuTreeNodes())
                {
                    if (nodesInfo.TryGetValue(child, out var existingInfo))
                    {
                        Debugger.LogWarning(
                            $"{child.Name} has already provided by {existingInfo.provider.Name}." +
                            $"Cannot be provided by {provider.Name} again!");
                        continue;
                    }

                    leftNodes.Enqueue(child);
                    nodes.Add(child);
                    
                    var parent = child.ParentNode;
                    if (parent == null && provider is IGameEditorMenuTreeNode providerNode)
                    {
                        parent = providerNode;
                    }
                    
                    var childInfo = SimplePool<TreeNodePreBuildInfo>.Get();
                    childInfo.node = child;
                    childInfo.parent = parent;
                    childInfo.provider = provider;
                    
                    nodesInfo.Add(child, childInfo);
                }
            }

            foreach (var (node, info) in nodesInfo)
            {
                if (info.parent == null)
                {
                    continue;
                }
                
                if (nodesInfo.ContainsKey(info.parent) == false)
                {
                    Debugger.LogWarning($"The parent of {node.Name} is not found!");
                    info.parent = info.provider as IGameEditorMenuTreeNode;
                }

                if (info.parent == null)
                {
                    continue;
                }
                
                var parentInfo = nodesInfo[info.parent];

                parentInfo.childrenInfos.Add(info);
                info.parentInfo = parentInfo;
            }

            foreach (var (node, info) in nodesInfo)
            {
                info.name = node.Name;
            }

            var nameDictionary = new Dictionary<string, HashSet<TreeNodePreBuildInfo>>();

            foreach (var (node, info) in nodesInfo)
            {
                if (info.childrenInfos.Count <= 0)
                {
                    continue;
                }
                
                nameDictionary.Clear();

                foreach (var childInfo in info.childrenInfos)
                {
                    if (childInfo.name == null)
                    {
                        continue;
                    }
                    
                    if (nameDictionary.TryGetValue(childInfo.name, out var hashSet) == false)
                    {
                        hashSet = SimplePool<HashSet<TreeNodePreBuildInfo>>.Get();
                        hashSet.Clear();
                        nameDictionary.Add(childInfo.name, hashSet);
                    }
                    
                    hashSet.Add(childInfo);
                }
                
                foreach (var childInfosByName in nameDictionary.Values)
                {
                    if (childInfosByName.Count <= 1)
                    {
                        continue;
                    }

                    int autoIndex = 0;
                    foreach (var childInfo in childInfosByName)
                    {
                        if (childInfo.node is IIDOwner<string> idOwner)
                        {
                            childInfo.name = $"{childInfo.name}({idOwner.id})";
                            continue;
                        }
                        
                        childInfo.name = $"{childInfo.name}({autoIndex})";
                        autoIndex++;
                    }
                }

                foreach (var childInfosByName in nameDictionary.Values)
                {
                    SimplePool<HashSet<TreeNodePreBuildInfo>>.Return(childInfosByName);
                }
                
                nameDictionary.Clear();
            }

            foreach (var (node, info) in nodesInfo)
            {
                var path = info.name;
                
                foreach (var parentInfo in info.TraverseToRoot(false))
                {
                    path = parentInfo.name + "/" + path;
                }

                if (node.IsVisible == false)
                {
                    continue;
                }
                
                tree.Add(path, node, node.Icon);
            }

            foreach (var info in nodesInfo.Values)
            {
                SimplePool<TreeNodePreBuildInfo>.Return(info);
                info.TryReset();
            }

            tree.DefaultMenuStyle.IconSize = 24.00f;
            tree.Config.DrawSearchToolbar = true;

            tree.EnumerateTree().Examine(AddRightClickContextMenu);

            return tree;
        }

        private void AddRightClickContextMenu(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += OnDrawRightClickContextMenu;
        }

        private void OnDrawRightClickContextMenu(OdinMenuItem menuItem)
        {
            if (menuItem.Value is not IGameEditorContextMenuProvider contextMenuProvider)
            {
                return;
            }

            if (Event.current.type != EventType.MouseDown || Event.current.button != 1 ||
                menuItem.Rect.Contains(Event.current.mousePosition) == false)
            {
                return;
            }

            GenericMenu menu = new GenericMenu();

            foreach (var config in contextMenuProvider.GetMenuItems())
            {
                menu.AddItem(new GUIContent(config.name, config.tooltip), false,
                    () => config.onClick?.Invoke());
            }

            var selectedNodes = MenuTree.Selection.SelectedValues.Cast<IGameEditorMenuTreeNode>()
                .WhereNotNull().ToList();

            foreach (var modifier in contextMenuModifiers)
            {
                modifier.ModifyContextMenu(contextMenuProvider, selectedNodes, menu);
            }

            menu.ShowAsContext();

            Event.current.Use();
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

            var label = $"{selected.Name}({selected.Value.GetType().Name})";
            GUILayout.Label(label);

            if (selected.Value is not IGameEditorToolbarProvider toolBarProvider)
            {
                SirenixEditorGUI.EndHorizontalToolbar();
                return;
            }
            
            var tree = new StringPathTree<ToolbarButtonConfig>();
                        
            foreach (var buttonConfig in toolBarProvider.GetToolbarButtons())
            {
                tree.Add(buttonConfig.path, buttonConfig);
            }

            foreach (var buttonNode in tree.Root.Children.Values)
            {
                if (SirenixEditorGUI.ToolbarButton(new GUIContent(buttonNode.PathPart,
                        buttonNode.data.tooltip)))
                {
                    if (buttonNode.Children.Count <= 0)
                    {
                        buttonNode.data.onClick?.Invoke();
                    }
                    else
                    {
                        GenericMenu menu = new GenericMenu();

                        Action action = null;
                        foreach (var leaf in buttonNode.GetAllLeaves(true))
                        {
                            menu.AddItem(new GUIContent(leaf.PathPart, leaf.data.tooltip), false, () =>
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

        private void OnProjectChange()
        {
            GamePrefabGeneralSettingUtility.RefreshAllInitialGamePrefabWrappers();
            
            ForceMenuTreeRebuild();
        }
    }
}
#endif