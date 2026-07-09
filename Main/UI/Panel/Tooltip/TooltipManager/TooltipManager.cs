using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public class TooltipManager : ManagerBehaviour<TooltipManager>
    {
        private static TooltipGeneralSetting Setting => UISetting.TooltipGeneralSetting;

        [ShowInInspector]
        private readonly Dictionary<string, List<ITooltip>> tooltipsLookup = new();

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            UIPanelManager.Instance.OnPanelCreatedEvent += OnPanelCreated;
            UIPanelManager.Instance.OnPanelDestructedEvent += OnPanelDestructed;
        }

        protected virtual void OnPanelCreated(IUIPanel panel)
        {
            if (panel.Modifiers.TryGetTarget(out ITooltip tooltip))
            {
                var tooltips =
                    tooltipsLookup.GetOrCreateFromFactory(panel.id, ListPoolFactory<ITooltip>.CreateFromDefaultPool);
                tooltips.Add(tooltip);
            }
        }

        protected virtual void OnPanelDestructed(IUIPanel panel)
        {
            if (tooltipsLookup.TryGetValue(panel.id, out var tooltips))
            {
                if (panel.Modifiers.TryGetTarget(out ITooltip tooltip))
                {
                    tooltips.Remove(tooltip);
                }
            }
        }

        public virtual bool Open(object target, IUIPanel source, TooltipOpenInfo info = default)
        {
            return Open(Setting.defaultTooltipID, target, source, info);
        }

        public virtual bool Open(string tooltipID, object target, IUIPanel source, TooltipOpenInfo info = default)
        {
            if (target == null)
            {
                return false;
            }

            if (tooltipID.IsNullOrEmpty())
            {
                return false;
            }

            if (tooltipsLookup.TryGetValue(tooltipID, out var tooltips) == false)
            {
                return false;
            }

            var tooltip = tooltips[0];

            tooltip.Open(target, source, info);

            return true;
        }

        public virtual void CloseAll(object target)
        {
            if (target == null)
            {
                UnityEngine.Debug.LogWarning($"{nameof(target)} is Null");
                return;
            }

            foreach (var tooltips in tooltipsLookup.Values)
            {
                foreach (var tooltip in tooltips)
                {
                    tooltip.Close(target);
                }
            }
        }

        public virtual void CloseDefault(object target)
        {
            Close(Setting.defaultTooltipID, target);
        }

        public virtual void Close(string tooltipID, object target)
        {
            if (tooltipID.IsNullOrEmpty())
            {
                return;
            }

            if (target == null)
            {
                UnityEngine.Debug.LogWarning($"{nameof(target)} is Null");
                return;
            }

            if (tooltipsLookup.TryGetValue(tooltipID, out var tooltips) == false)
            {
                return;
            }

            foreach (var tooltip in tooltips)
            {
                tooltip.Close(target);
            }
        }
    }
}