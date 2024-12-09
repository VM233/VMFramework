#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Configuration;

namespace VMFramework.GameLogicArchitecture
{
    [HideDuplicateReferenceBox]
    [HideReferenceObjectPicker]
    [OnInspectorInit("@((IInspectorConfig)$value)?.OnInspectorInit()")]
    public partial class SubGamePrefab
    {
        protected virtual void OnInspectorInit()
        {
            gameTags ??= new();
        }
        
        void IInspectorConfig.OnInspectorInit()
        {
            OnInspectorInit();
        }
    }
}
#endif