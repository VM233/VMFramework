#if UNITY_EDITOR
namespace VMFramework.Procedure 
{
    public partial class DefaultGlobalScenesGeneralSetting
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            sceneNames ??= new();
        }
    }
}
#endif