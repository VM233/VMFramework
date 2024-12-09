#if UNITY_EDITOR
namespace VMFramework.Configuration
{
    public partial class GameTagBasedConfigBase
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            gameTags ??= new();
        }
    }
}
#endif