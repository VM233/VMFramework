using UnityEngine;

namespace VMFramework.Configuration
{
    public abstract class GeneralConfig : ScriptableObject, IConfig
    {
        [field: System.NonSerialized]
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
