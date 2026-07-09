#if UNITY_EDITOR
namespace VMFramework.UI
{
    public partial class UIPanelProcedureConfig
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            uniqueUIPanelAutoOpenOnEnter ??= new();
            uiPanelAutoCloseOnEnter ??= new();
            uniqueUIPanelAutoOpenOnExit ??= new();
            uiPanelAutoCloseOnExit ??= new();
        }
    }
}
#endif