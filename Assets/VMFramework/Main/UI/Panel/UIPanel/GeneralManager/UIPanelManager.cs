using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;
using VMFramework.Procedure;
using VMFramework.Tools;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public sealed partial class UIPanelManager : ManagerBehaviour<UIPanelManager>
    {
        public static Transform UIContainer { get; private set; }

        public static event Action<IUIPanel> OnPanelCreatedEvent;

        public static event Action<IUIPanel> OnPanelDestructedEvent;

        [ShowInInspector]
        private static readonly Dictionary<string, IUIPanel> uniquePanels = new();
        
        [ShowInInspector]
        private static readonly Dictionary<string, HashSet<IUIPanel>> openedPanels = new();

        [ShowInInspector]
        private static readonly HashSet<IUIPanel> preOpeningPanels = new();

        private static readonly HashSet<IUIPanel> preOpeningPanelsCache = new();
        
        [ShowInInspector]
        private static readonly HashSet<IUIPanel> closingPanels = new();

        private static readonly List<IUIPanel> closedPanels = new();

        #region Init

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            UIContainer = ContainerTransform.Get(UISetting.UIPanelGeneralSetting.containerName);
        }

        #endregion

        #region Register & Unregister

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Register(IUIPanel panel)
        {
            // Debugger.LogWarning($"Registering UI panel with ID {panel.id}");
            
            var id = panel.id;

            if (panel.IsUnique)
            {
                if (uniquePanels.ContainsKey(id))
                {
                    Debugger.LogWarning($"The unique UI panel with ID {id} has already been created, " +
                                        "the old panel will be overwritten");
                }

                uniquePanels[id] = panel;
            }

            OnPanelCreatedEvent?.Invoke(panel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Unregister(IUIPanel panel)
        {
            // Debugger.LogWarning($"Unregistering UI panel with ID {panel.id}");
            
            var id = panel.id;

            if (panel.IsUnique)
            {
                if (uniquePanels.ContainsKey(id) == false)
                {
                    Debugger.LogWarning($"The unique UI panel with ID {id} has not been created, " +
                                        "cannot unregister it");
                    return;
                }

                uniquePanels.Remove(id);
            }

            OnPanelDestructedEvent?.Invoke(panel);
        }

        #endregion

        #region Open & Close

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Toggle(IUIPanel panel)
        {
            if (panel.IsOpened)
            {
                Close(panel);
            }
            else
            {
                Open(panel, null);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Open(IUIPanel panel, IUIPanel source)
        {
            TryOpen(panel, source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryOpen(IUIPanel panel, IUIPanel source)
        {
            if (closingPanels.Contains(panel))
            {
                if (preOpeningPanels.Add(panel) == false)
                {
                    Debugger.LogWarning($"The UI panel: {panel.id} is already pre-opening, cannot open it again");
                }
                
                return false;
            }

            panel.OnOpen(source);

            var set = openedPanels.GetOrAddFromDefaultPool(panel.id);
            
            set.Add(panel);

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryClose(IUIPanel panel)
        {
            if (panel.IsOpened == false)
            {
                return false;
            }
            
            if (closingPanels.Add(panel) == false)
            {
                if (preOpeningPanels.Remove(panel) == false)
                {
                    Debugger.LogWarning($"The UI panel with ID {panel.id} is already closing, cannot close it again");
                }
                
                return false;
            }

            panel.OnClose();

            if (openedPanels.TryGetValue(panel.id, out var set))
            {
                set.Remove(panel);
            }
            else
            {
                Debugger.LogWarning($"The UI panel with ID {panel.id} is not opened!");
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Close(IUIPanel panel)
        {
            TryClose(panel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsClosing(IUIPanel panel) => closingPanels.Contains(panel);

        private void Update()
        {
            if (closingPanels.Count > 0)
            {
                foreach (var closingPanel in closingPanels)
                {
                    if (closingPanel.IsOpened == false)
                    {
                        closedPanels.Add(closingPanel);
                    }
                }

                if (closedPanels.Count > 0)
                {
                    foreach (var closedPanel in closedPanels)
                    {
                        closedPanel.OnPostClose();
                        closingPanels.Remove(closedPanel);

                        if (closedPanel.IsUnique == false)
                        {
                            GameItemManager.Return(closedPanel);
                        }
                    }

                    closedPanels.Clear();
                }
            }

            if (preOpeningPanels.Count > 0)
            {
                foreach (var preOpeningPanel in preOpeningPanels)
                {
                    if (preOpeningPanel.IsOpened)
                    {
                        Debugger.LogWarning($"The UI panel with ID {preOpeningPanel.id} is already opened, " +
                                            $"cannot pre-open it again");
                        
                        continue;
                    }
                    
                    TryOpen(preOpeningPanel, null);
                    
                    preOpeningPanelsCache.Add(preOpeningPanel);
                }

                if (preOpeningPanelsCache.Count > 0)
                {
                    foreach (var preOpeningPanel in preOpeningPanelsCache)
                    {
                        preOpeningPanels.Remove(preOpeningPanel);
                    }
                    
                    preOpeningPanelsCache.Clear();
                }
            }
        }

        #endregion

        #region Opened Panels

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetOpenedPanels(string id, out IReadOnlyCollection<IUIPanel> panels)
        {
            if (openedPanels.TryGetValue(id, out var set))
            {
                if (set.Count > 0)
                {
                    panels = set;
                    return true;
                }
            }
            
            panels = null;
            return false;
        }

        #endregion

        #region Unique Panel

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetUniquePanels<TPanel>(ICollection<TPanel> panels)
        {
            foreach (var panel in uniquePanels.Values)
            {
                if (panel is TPanel typedPanel)
                {
                    panels.Add(typedPanel);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TPanel GetUniquePanel<TPanel>(string id)
            where TPanel : IUIPanel
        {
            if (uniquePanels.TryGetValue(id, out var panel) == false)
            {
                return default;
            }

            if (panel is TPanel typedPanel)
            {
                return typedPanel;
            }

            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IUIPanel GetUniquePanelStrictly(string id)
        {
            var panelController = uniquePanels.GetValueOrDefault(id);
            panelController.AssertIsNotNull(nameof(panelController));
            return panelController;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TPanel GetUniquePanelStrictly<TPanel>(string id)
            where TPanel : IUIPanel
        {
            var panel = GetUniquePanel<TPanel>(id);

            if (panel == null)
            {
                throw new Exception($"The unique panel with ID {id} does not exist");
            }

            return panel;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetUniquePanel(string id, out IUIPanel panel)
        {
            return uniquePanels.TryGetValue(id, out panel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetUniquePanel<TPanel>(string id, out TPanel panel)
            where TPanel : IUIPanel
        {
            if (uniquePanels.TryGetValue(id, out var panelInterface))
            {
                if (panelInterface is TPanel typedPanel)
                {
                    panel = typedPanel;
                    return true;
                }
            }

            panel = default;
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetUniquePanelWithWarning(string id, out IUIPanel panel)
        {
            if (uniquePanels.TryGetValue(id, out panel) == false)
            {
                Debugger.LogWarning($"The unique panel with ID {id} does not exist");
                return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetUniquePanelWithWarning<TPanel>(string id, out TPanel panel)
            where TPanel : IUIPanel
        {
            if (uniquePanels.TryGetValue(id, out var panelInterface) == false)
            {
                Debugger.LogWarning($"The unique panel with ID {id} does not exist");
                panel = default;
                return false;
            }

            if (panelInterface is not TPanel typedPanel)
            {
                Debugger.LogWarning($"The unique panel with ID {id} is {panelInterface.GetType()} " +
                                    $"instead of {typeof(TPanel)}");
                panel = default;
                return false;
            }

            panel = typedPanel;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IUIPanel GetAndOpenUniquePanel(string id)
        {
            if (uniquePanels.TryGetValue(id, out var panel))
            {
                TryOpen(panel, null);
                return panel;
            }

            return null;
        }

        #endregion

        #region Get And Open

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IUIPanel GetAndOpen(string id)
        {
            if (uniquePanels.TryGetValue(id, out var panel))
            {
                TryOpen(panel, null);
                return panel;
            }
            
            return CreateAndOpen(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TPanel GetAndOpen<TPanel>(string id) 
            where TPanel : IUIPanel
        {
            if (uniquePanels.TryGetValue(id, out var panel))
            {
                if (panel is not TPanel typedPanel)
                {
                    throw new ArgumentException($"The unique panel with ID {id} is not of type {typeof(TPanel)}");
                }
                
                TryOpen(panel, null);
                return typedPanel;
            }
            
            return CreateAndOpen<TPanel>(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IUIPanel CreateAndOpen(string id)
        {
            var panel = GameItemManager.Get<IUIPanel>(id);

            TryOpen(panel, null);

            return panel;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TPanel CreateAndOpen<TPanel>(string id)
            where TPanel : IUIPanel
        {
            var panel = GameItemManager.Get<TPanel>(id);
            
            TryOpen(panel, null);

            return panel;
        }

        #endregion
    }
}