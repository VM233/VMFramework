using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public sealed class TooltipManager : ManagerBehaviour<TooltipManager>
    {
        private static TooltipGeneralSetting tooltipGeneralSetting => UISetting.TooltipGeneralSetting;

        private static readonly List<ITooltip> tooltipsCache = new();
        
        public static void Open(ITooltipProvider tooltipProvider, IUIPanel source)
        {
            if (tooltipProvider == null)
            {
                Debugger.LogWarning($"{nameof(tooltipProvider)} is Null");
                return;
            }

            if (tooltipProvider.ShowTooltip() == false)
            {
                return;
            }

            string tooltipID = null;
            TooltipOpenInfo info = new();
            
            bool priorityFound = false;

            if (tooltipProvider is IGameTagsOwner gameTagsOwner)
            {
                if (tooltipGeneralSetting.tooltipIDBindConfigs.TryGetConfigRuntime(
                        gameTagsOwner.GameTags, out var tooltipBindConfig))
                {
                    tooltipID = tooltipBindConfig.tooltipID;
                }

                if (tooltipGeneralSetting.tooltipPriorityBindConfigs.TryGetConfigRuntime(
                        gameTagsOwner.GameTags, out var priorityBindConfig))
                {
                    info.priority = priorityBindConfig.priority;
                    priorityFound = true;
                }
            }
            
            tooltipID ??= tooltipGeneralSetting.defaultTooltipID;

            if (priorityFound == false)
            {
                info.priority = tooltipGeneralSetting.defaultPriority;
            }

            if (UIPanelManager.TryGetUniquePanelWithWarning(tooltipID, out ITooltip tooltip) == false)
            {
                return;
            }

            tooltip.Open(tooltipProvider, source, info);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Close(ITooltipProvider tooltipProvider)
        {
            if (tooltipProvider == null)
            {
                Debugger.LogWarning($"{nameof(tooltipProvider)} is Null");
                return;
            }

            tooltipsCache.Clear();
            UIPanelManager.GetUniquePanels(tooltipsCache);
            foreach (var tooltip in tooltipsCache)
            {
                tooltip.Close(tooltipProvider);
            }
        }
    }
}