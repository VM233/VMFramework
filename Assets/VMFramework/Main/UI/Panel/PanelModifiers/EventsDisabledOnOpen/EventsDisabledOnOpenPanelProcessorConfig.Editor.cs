#if UNITY_EDITOR
namespace VMFramework.UI
{
    public partial class EventsDisabledOnOpenPanelModifierConfig
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            gameEventDisabledOnOpen ??= new();
        }
    }
}
#endif