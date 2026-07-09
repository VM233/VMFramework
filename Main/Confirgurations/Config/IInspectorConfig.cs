#if UNITY_EDITOR
namespace VMFramework.Configuration
{
    public interface IInspectorConfig
    {
        public void OnInspectorInit();
    }
}
#endif