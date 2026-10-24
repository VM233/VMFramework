using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    [ManagerCreationProvider(ManagerType.UICore)]
    public class TooltipManager : ManagerBehaviour<TooltipManager>
    {
        private static TooltipGeneralSetting Setting => UISetting.TooltipGeneralSetting;

        [ShowInInspector]
        private readonly Dictionary<string, Dictionary<IUIPanel, ITooltip>> tooltipsLookup = new();
        private UIPanelManager panelOwner;

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            panelOwner = UIPanelManager.Instance;
            panelOwner.OnPanelCreatedEvent += OnPanelCreated;
            panelOwner.OnPanelDestructedEvent += OnPanelDestructed;
        }

        protected override void OnDestroy()
        {
            if (!ReferenceEquals(panelOwner, null))
            {
                panelOwner.OnPanelCreatedEvent -= OnPanelCreated;
                panelOwner.OnPanelDestructedEvent -= OnPanelDestructed;
                panelOwner = null;
            }
            tooltipsLookup.Clear();
            base.OnDestroy();
        }

        protected virtual void OnPanelCreated(IUIPanel panel)
        {
            if (panel.Modifiers.TryGetTarget(out ITooltip tooltip))
            {
                var tooltips =
                    tooltipsLookup.GetOrCreateFromFactory(panel.id, () => new Dictionary<IUIPanel, ITooltip>());
                tooltips.Add(panel, tooltip);
            }
        }

        protected virtual void OnPanelDestructed(IUIPanel panel)
        {
            if (tooltipsLookup.TryGetValue(panel.id, out var tooltips))
            {
                tooltips.Remove(panel);
                if (tooltips.Count == 0) tooltipsLookup.Remove(panel.id);
            }
        }

        public virtual bool Open(object target, IUIPanel source, TooltipOpenInfo info = default)
        {
            return Open(Setting.DefaultTooltipID, target, source, info);
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

            var tooltip = tooltips.Values.First();

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
                foreach (var tooltip in tooltips.Values)
                {
                    tooltip.Close(target);
                }
            }
        }

        public virtual void CloseDefault(object target)
        {
            Close(Setting.DefaultTooltipID, target);
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

            foreach (var tooltip in tooltips.Values)
            {
                tooltip.Close(target);
            }
        }
    }
}
