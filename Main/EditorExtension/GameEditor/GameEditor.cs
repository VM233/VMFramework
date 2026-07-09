#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.GameLogicArchitecture;
using VMFramework.GameLogicArchitecture.Editor;
using VMFramework.Procedure.Editor;
using Object = UnityEngine.Object;

namespace VMFramework.Editor.GameEditor
{
    public class GameEditor : OdinMenuEditorWindow
    {
        protected static readonly FuncTargetsProcessorPipeline<ContextMenuItemConfig> contextMenuProcessors = new();
        protected static readonly FuncProcessorPipeline<ToolbarButtonConfig> toolbarButtonProcessors = new(); 

        private static readonly List<ToolbarButtonConfig> toolbarButtonConfigsCache = new();
        private static readonly List<ContextMenuItemConfig> contextMenuItemConfigsCache = new();

        private readonly Dictionary<object, object> visualNodeParentsLookup = new();

        [MenuItem(UnityMenuItemNames.VMFRAMEWORK + GameEditorNames.GAME_EDITOR + " #G", false, 100)]
        private static void OpenWindow()
        {
            var window = CreateWindow<GameEditor>(GameEditorNames.GAME_EDITOR);
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            contextMenuProcessors.AutoCollect();
            toolbarButtonProcessors.AutoCollect();
            
            if (EditorInitializer.IsInitialized == false && Application.isPlaying == false)
            {
                var loadingPreviewTree = new OdinMenuTree(true)
                {
                    { "Loading...", new GameEditorLoadingPreview() }
                };
                
                loadingPreviewTree.Selection.Add(loadingPreviewTree.RootMenuItem.ChildMenuItems[0]);
                
                return loadingPreviewTree;
            }

            OdinMenuTree tree = new(true);

            var nodesInfo = new Dictionary<object, TreeNodePreBuildInfo>();
            
            var leftNodes = new Queue<object>();
            
            foreach (var globalSettingFile in GlobalSettingFileEditorManager.GetGlobalSettings())
            {
                if (globalSettingFile is IGameEditorMenuTreeNode node)
                {
                    leftNodes.Enqueue(node);
                    var info = new TreeNodePreBuildInfo
                    {
                        node = node
                    };
                    nodesInfo.Add(node, info);
                }
            }

            int loopCount = 0;
            while (leftNodes.Count > 0)
            {
                loopCount++;

                if (loopCount > 10000)
                {
                    Debug.LogError("Infinite loop detected while building menu tree!");
                    break;
                }
                
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
                    if (child.IsUnityNull())
                    {
                        continue;
                    }
                    
                    if (nodesInfo.TryGetValue(child, out var existingInfo))
                    {
                        UnityEngine.Debug.LogWarning(
                            $"{child} has already provided by {existingInfo.provider.Name}." +
                            $"Cannot be provided by {provider.Name} again!");
                        continue;
                    }

                    leftNodes.Enqueue(child);
                    
                    object parent = provider;
                    if (child is IGameEditorMenuTreeNode menuTreeNode && menuTreeNode.ParentNode.IsUnityNull() == false)
                    {
                        parent = menuTreeNode.ParentNode;
                    }

                    var childInfo = new TreeNodePreBuildInfo
                    {
                        node = child,
                        parent = parent,
                        provider = provider
                    };

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
                    UnityEngine.Debug.LogWarning($"The parent of {node} is not found!");
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
                if (node is INameOwner nameOwner)
                {
                    info.name = nameOwner.Name;
                }

                if (info.name.IsNullOrEmpty() == false)
                {
                    continue;
                }
                
                if (node is IIDOwner<string> idOwner)
                {
                    info.name = idOwner.id;
                }

                if (info.name.IsNullOrEmpty() == false)
                {
                    continue;
                }
                
                info.name = node.ToString();
            }

            var nameDictionary = new Dictionary<string, HashSet<TreeNodePreBuildInfo>>();

            foreach (var (_, info) in nodesInfo)
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

                    var infos = nameDictionary.GetOrCreate(childInfo.name);
                    
                    infos.Add(childInfo);
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
            }

            visualNodeParentsLookup.Clear();
            foreach (var (node, info) in nodesInfo)
            {
                var path = info.name;
                
                foreach (var parentInfo in info.TraverseToRoot(false))
                {
                    path = parentInfo.name + "/" + path;
                }

                object visualNode = node;
                EditorIcon icon = EditorIcon.None;

                if (node is IGameEditorMenuTreeNode menuTreeNode)
                {
                    if (menuTreeNode.VisualNode.IsUnityNull() == false)
                    {
                        visualNode = menuTreeNode.VisualNode;
                        visualNodeParentsLookup.Add(visualNode, info.node);
                    }
                    
                    icon = menuTreeNode.Icon;
                }
                
                tree.Add(path, visualNode, icon);
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
            if (Event.current.type != EventType.MouseDown || Event.current.button != 1 ||
                menuItem.Rect.Contains(Event.current.mousePosition) == false)
            {
                return;
            }

            var items = MenuTree.Selection.SelectedValues.ToList();
            contextMenuItemConfigsCache.Clear();
            if (contextMenuProcessors.ProcessTargets(items, contextMenuItemConfigsCache) == false)
            {
                return;
            }

            GenericMenu menu = new GenericMenu();

            foreach (var config in contextMenuItemConfigsCache)
            {
                menu.AddItem(new GUIContent(config.name, config.tooltip), false,
                    () => config.onClick?.Invoke());
            }

            menu.ShowAsContext();

            Event.current.Use();
        }

        protected override void OnBeginDrawEditors()
        {
            var selected = MenuTree?.Selection?.FirstOrDefault();
            
            if (selected is not { Value: not null })
            {
                return;
            }

            var label = $"{selected.Name}";
            
            var value = selected.Value;

            if (visualNodeParentsLookup.TryGetValue(value, out var parent))
            {
                value = parent;
            }
            
            toolbarButtonConfigsCache.Clear();
            if (toolbarButtonProcessors.ProcessTarget(value, toolbarButtonConfigsCache) == false)
            {
                return;
            }
            
            var toolbarHeight = MenuTree.Config.SearchToolbarHeight;
            
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            GUILayout.Label(label);
            
            var tree = new StringPathTree<ToolbarButtonConfig>();
            
            foreach (var buttonConfig in toolbarButtonConfigsCache)
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
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            GUILayout.Label($"Type: {selected.Value.GetType().Name}");
            if (selected.Value is Object obj)
            {
                GUILayout.Label($"Object Name: {obj.name}", SirenixGUIStyles.RightAlignedWhiteMiniLabel);
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