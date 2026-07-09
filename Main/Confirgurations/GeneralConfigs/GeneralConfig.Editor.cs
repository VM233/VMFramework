#if UNITY_EDITOR
namespace VMFramework.Configuration
{
    public partial class GeneralConfig
    {
        protected virtual void OnInspectorInit()
        {
            
        }

        void IInspectorConfig.OnInspectorInit()
        {
            OnInspectorInit();
        }
    }
}
#endif