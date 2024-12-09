using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.Containers;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.ContainerUICore)]
    public sealed partial class ContainerUIManager : ManagerBehaviour<ContainerUIManager>
    {
        #region Init

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            UIPanelManager.OnPanelCreatedEvent += OnPanelCreated;
        }

        #endregion

        #region Panel Create Event

        private static void OnPanelCreated(IUIPanel panel)
        {
            if (panel is not IContainerUIPanel)
            {
                return;
            }
            
            panel.OnOpenEvent += OnOpenContainerUIPriorityRegistered;
            panel.OnPostCloseEvent += OnCloseContainerUIPriorityUnregistered;
            panel.OnDestructEvent += OnCloseContainerUIPriorityUnregistered;

            panel.OnOpenEvent += OnPanelOpenCollectorRegister;
            panel.OnPostCloseEvent += OnPanelCloseCollectorUnregister;
            panel.OnDestructEvent += OnPanelCloseCollectorUnregister;
        }

        #endregion

        #region Open Container UI & Close

        public static IUIPanel OpenUI(IContainer container, IUIPanel source = null)
        {
            container.AssertIsNotNull(nameof(container));

            if (UISetting.ContainerUIGeneralSetting.containerUIBindConfigs.TryGetConfigRuntimeWithWarning(container.id,
                    out var bindConfig) == false)
            {
                return null;
            }

            var containerUIPanel = UIPanelManager.GetUniquePanelStrictly<IContainerUIPanel>(bindConfig.uiID);

            containerUIPanel.Open(source);
            containerUIPanel.AddBindContainer(container);

            return containerUIPanel;
        }

        public static void CloseUI(IContainer container)
        {
            container.AssertIsNotNull(nameof(container));

            if (UISetting.ContainerUIGeneralSetting.containerUIBindConfigs.TryGetConfigRuntimeWithWarning(container.id,
                    out var bindConfig) == false)
            {
                return;
            }

            var containerUIPanel = UIPanelManager.GetUniquePanelStrictly<IContainerUIPanel>(bindConfig.uiID);

            containerUIPanel.Close();
        }

        #endregion

        #region Open Container Owner & Close

        private static readonly List<IContainer> containersCache = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OpenContainerOwner(IContainerOwner owner)
        {
            containersCache.Clear();
            owner.GetContainers(containersCache);
            foreach (var container in containersCache)
            {
                OpenUI(container);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CloseContainerOwner(IContainerOwner owner)
        {
            containersCache.Clear();
            owner.GetContainers(containersCache);
            foreach (var container in containersCache)
            {
                CloseUI(container);
            }
        }

        #endregion

        #region Container UI Priority

        [ShowInInspector]
        private static Dictionary<int, IContainerUIPanel> containerUIPriorityDict = new();

        public static void OnOpenContainerUIPriorityRegistered(IUIPanel uiPanelController)
        {
            if (uiPanelController is not IContainerUIPanel containerUIPanel)
            {
                return;
            }

            containerUIPriorityDict[containerUIPanel.ContainerUIPriority] = containerUIPanel;
        }

        public static void OnCloseContainerUIPriorityUnregistered(IUIPanel uiPanelController)
        {
            if (uiPanelController is not IContainerUIPanel containerUIPanel)
            {
                return;
            }

            if (containerUIPriorityDict.TryGetValue(containerUIPanel.ContainerUIPriority,
                    out var existedContainerUIPanel))
            {
                if (existedContainerUIPanel == containerUIPanel)
                {
                    containerUIPriorityDict.Remove(containerUIPanel.ContainerUIPriority);
                }
            }
        }

        [Button]
        public static IContainerUIPanel GetHighestPriorityContainerUIPanel()
        {
            if (containerUIPriorityDict.Count == 0)
            {
                return null;
            }

            var highestPriority = containerUIPriorityDict.Keys.Max();

            return containerUIPriorityDict[highestPriority];
        }

        [Button]
        public static IContainerUIPanel GetSecondHighestPriorityContainerUIPanel()
        {
            if (containerUIPriorityDict.Count == 0)
            {
                return null;
            }

            var (highestPriority, secondHighestPriority) = containerUIPriorityDict.Keys.TwoMaxValues();

            return containerUIPriorityDict[secondHighestPriority];
        }

        #endregion
    }
}