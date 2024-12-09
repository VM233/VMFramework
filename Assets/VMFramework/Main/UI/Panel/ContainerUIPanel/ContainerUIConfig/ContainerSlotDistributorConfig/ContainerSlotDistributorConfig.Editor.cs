#if UNITY_EDITOR
namespace VMFramework.UI
{
    public partial class ContainerSlotDistributorConfig
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            slotIndexRange ??= new();
        }
    }
}
#endif