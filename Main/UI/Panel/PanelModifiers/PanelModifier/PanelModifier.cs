using UnityEngine;
using VMFramework.Core;

namespace VMFramework.UI
{
    public abstract class PanelModifier : MonoBehaviour, IPanelModifier
    {
        public IUIPanel Panel { get; private set; }

        public bool IsInitialized { get; private set; } = false;

        protected IUIPanelConfig UIPanelConfig { get; private set; }

        public int InitializePriority { get; set; } = PriorityDefines.MEDIUM;

        public void Initialize(IUIPanel panel, IUIPanelConfig config)
        {
            Panel = panel;
            UIPanelConfig = config;
            OnInitialize();
            IsInitialized = true;
        }

        protected virtual void OnInitialize()
        {

        }

        public void Deinitialize()
        {
            OnDeinitialize();
            IsInitialized = false;
        }

        protected virtual void OnDeinitialize()
        {

        }
    }
}