using UnityEngine;

namespace VMFramework.Configuration
{
    public abstract partial class GeneralConfig : ScriptableObject, IConfig
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
    }
}