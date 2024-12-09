using System.Runtime.CompilerServices;

namespace VMFramework.UI
{
    public partial class TracingUIManager
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IUIPanel OpenOn(string panelID, TracingConfig config, out ITracingPanelModifier tracingProcessor)
        {
            var panel = UIPanelManager.GetAndOpen(panelID);

            foreach (var processor in panel.Modifiers)
            {
                if (processor is ITracingPanelModifier tracingUIProcessor)
                {
                    StartTracing(tracingUIProcessor, config);
                    tracingProcessor = tracingUIProcessor;
                    return panel;
                }
            }

            tracingProcessor = null;
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TPanel OpenOn<TPanel>(string panelID, TracingConfig config,
            out ITracingPanelModifier tracingProcessor)
            where TPanel : IUIPanel
        {
            var panel = UIPanelManager.GetAndOpen<TPanel>(panelID);
            
            foreach (var processor in panel.Modifiers)
            {
                if (processor is ITracingPanelModifier tracingUIProcessor)
                {
                    StartTracing(tracingUIProcessor, config);
                    tracingProcessor = tracingUIProcessor;
                    return panel;
                }
            }

            tracingProcessor = null;
            return default;
        }
    }
}