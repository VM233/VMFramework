using Newtonsoft.Json;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

namespace VMFramework.Configuration
{
    [System.Serializable]
    [JsonObject(MemberSerialization.OptIn, ItemTypeNameHandling = TypeNameHandling.All)]
#if UNITY_EDITOR
    [HideDuplicateReferenceBox]
    [HideReferenceObjectPicker]
    [OnInspectorInit("@((IInspectorConfig)$value)?.OnInspectorInit()")]
#endif
    public abstract class BaseConfig : IConfig
    {
        public bool InitDone { get; private set; } = false;

        public virtual void CheckSettings()
        {

        }

        public void Init()
        {
            OnInit();

            InitDone = true;
        }

        protected virtual void OnInit()
        {

        }
#if UNITY_EDITOR
        protected virtual void OnInspectorInit()
        {

        }

        void IInspectorConfig.OnInspectorInit()
        {
            OnInspectorInit();
        }
#endif
    }
}
