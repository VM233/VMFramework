#if UNITY_EDITOR
namespace VMFramework.UI
{
    public partial class UIToolkitContextMenuConfig
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            gameEventIDsToClose ??= new();
        }
    }
}
#endif