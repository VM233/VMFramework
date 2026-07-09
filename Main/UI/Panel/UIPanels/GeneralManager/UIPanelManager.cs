using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;
using VMFramework.Tools;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public partial class UIPanelManager : ManagerBehaviour<UIPanelManager>
    {
        public Transform UIContainer { get; private set; }

        public event Action<IUIPanel> OnPanelCreatedEvent;

        public event Action<IUIPanel> OnPanelDestructedEvent;

        [ShowInInspector]
        private readonly Dictionary<string, IUIPanel> uniquePanels = new();
        
        [ShowInInspector]
        private readonly Dictionary<string, HashSet<IUIPanel>> openedPanels = new();

        [ShowInInspector]
        private readonly HashSet<IUIPanel> preOpeningPanels = new();

        private readonly HashSet<IUIPanel> preOpeningPanelsCache = new();
        
        [ShowInInspector]
        private readonly HashSet<IUIPanel> closingPanels = new();

        private readonly List<IUIPanel> closedPanels = new();

        #region Init

        protected override void Awake()
        {
            base.Awake();

            OnPanelCreatedEvent = null;
            OnPanelDestructedEvent = null;
            
            uniquePanels.Clear();
            openedPanels.Clear();
            preOpeningPanels.Clear();
            preOpeningPanelsCache.Clear();
            closingPanels.Clear();
            closedPanels.Clear();
        }

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            UIContainer = ContainerTransform.Get(UISetting.UIPanelGeneralSetting.containerName);
        }

        #endregion

        #region Register & Unregister

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Register(IUIPanel panel)
        {
            var id = panel.id;

            if (panel.IsUnique)
            {
                if (uniquePanels.ContainsKey(id))
                {
                    UnityEngine.Debug.LogWarning($"The unique UI panel with ID {id} has already been created, " +
                                        "the old panel will be overwritten");
                }

                uniquePanels[id] = panel;
            }

            OnPanelCreatedEvent?.Invoke(panel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unregister(IUIPanel panel)
        {
            var id = panel.id;

            if (panel.IsUnique)
            {
                if (uniquePanels.Remove(id) == false)
                {
                    UnityEngine.Debug.LogWarning($"The unique UI panel with ID {id} has not been created, " +
                                        "cannot unregister it");
                    return;
                }
            }

            OnPanelDestructedEvent?.Invoke(panel);
        }

        #endregion

        #region Open & Close

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Toggle(IUIPanel panel)
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
        public void Open(IUIPanel panel, IUIPanel source)
        {
            TryOpen(panel, source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryOpen(IUIPanel panel, IUIPanel source)
        {
            // if (preOpeningPanels.Add(panel) == false)
            // {
            //     UnityEngine.Debug.LogWarning($"The UI panel: {panel.id} is already pre-opening, cannot open it again");
            //     return false;
            // }
            
            if (closingPanels.Remove(panel))
            {
                OnPanelClosing(panel);
            }

            panel.OnOpenInternal(source);

            var set = openedPanels.GetOrCreateFromFactory(panel.id, HashSetPoolFactory<IUIPanel>.CreateFromDefaultPool);
            
            set.Add(panel);

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryClose(IUIPanel panel)
        {
            if (panel.IsOpened == false)
            {
                return false;
            }
            
            if (closingPanels.Add(panel) == false)
            {
                if (preOpeningPanels.Remove(panel) == false)
                {
                    UnityEngine.Debug.LogWarning($"The UI panel with ID {panel.id} is already closing, cannot close it again");
                }
                
                return false;
            }

            panel.OnPreCloseInternal();

            if (openedPanels.TryGetValue(panel.id, out var set))
            {
                set.Remove(panel);
            }
            else
            {
                UnityEngine.Debug.LogWarning($"The UI panel with ID {panel.id} is not opened!");
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Close(IUIPanel panel)
        {
            TryClose(panel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsClosing(IUIPanel panel) => closingPanels.Contains(panel);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnPanelClosing(IUIPanel panel)
        {
            panel.OnPostCloseInternal();

            if (panel.IsUnique == false)
            {
                GameItemManager.Instance.Return(panel);
            }
        }

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
                        OnPanelClosing(closedPanel);
                        
                        closingPanels.Remove(closedPanel);
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
                        UnityEngine.Debug.LogWarning($"The UI panel with ID {preOpeningPanel.id} is already opened, " +
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
        public bool TryGetOpenedPanels(string id, out IReadOnlyCollection<IUIPanel> panels)
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
        public void GetUniquePanels<TPanel>(ICollection<TPanel> panels)
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
        public TPanel GetUniquePanel<TPanel>(string id)
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
        public IUIPanel GetUniquePanelStrictly(string id)
        {
            var panelController = uniquePanels.GetValueOrDefault(id);
            panelController.AssertIsNotNull(nameof(panelController));
            return panelController;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TPanel GetUniquePanelStrictly<TPanel>(string id)
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
        public bool TryGetUniquePanel(string id, out IUIPanel panel)
        {
            return uniquePanels.TryGetValue(id, out panel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetUniquePanel<TPanel>(string id, out TPanel panel)
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
        public bool TryGetUniquePanelWithWarning(string id, out IUIPanel panel)
        {
            if (uniquePanels.TryGetValue(id, out panel) == false)
            {
                UnityEngine.Debug.LogWarning($"The unique panel with ID {id} does not exist");
                return false;
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetUniquePanelWithWarning<TPanel>(string id, out TPanel panel)
            where TPanel : IUIPanel
        {
            if (uniquePanels.TryGetValue(id, out var panelInterface) == false)
            {
                UnityEngine.Debug.LogWarning($"The unique panel with ID {id} does not exist");
                panel = default;
                return false;
            }

            if (panelInterface is not TPanel typedPanel)
            {
                UnityEngine.Debug.LogWarning($"The unique panel with ID {id} is {panelInterface.GetType()} " +
                                    $"instead of {typeof(TPanel)}");
                panel = default;
                return false;
            }

            panel = typedPanel;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IUIPanel GetAndOpenUniquePanel(string id)
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
        public IUIPanel GetAndOpen(string id, IUIPanel source = null)
        {
            if (uniquePanels.TryGetValue(id, out var panel))
            {
                TryOpen(panel, source);
                return panel;
            }
            
            return CreateAndOpen(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TPanel GetAndOpen<TPanel>(string id) 
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
        public IUIPanel CreateAndOpen(string id)
        {
            var panel = GameItemManager.Instance.Get<IUIPanel>(id);

            TryOpen(panel, null);

            return panel;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TPanel CreateAndOpen<TPanel>(string id)
            where TPanel : IUIPanel
        {
            var panel = GameItemManager.Instance.Get<TPanel>(id);
            
            TryOpen(panel, null);

            return panel;
        }

        #endregion

        #region Reenter

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Reenter(string id)
        {
            if (uniquePanels.TryGetValue(id, out var panel))
            {
                panel.Reenter();
            }
        }

        #endregion
    }
}