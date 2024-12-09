#if UNITY_EDITOR
namespace VMFramework.UI
{
    public partial class EventsDisabledOnMouseEnterContainerPanelModifierConfig
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            gameEventsDisabledOnMouseEnter ??= new();
        }
    }
}
#endif