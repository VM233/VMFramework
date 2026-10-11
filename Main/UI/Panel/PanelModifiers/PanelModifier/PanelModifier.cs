using System;
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
            try { OnInitialize(); IsInitialized = true; }
            catch (Exception primary)
            {
                var failures = new UIPanelReleaseFailures(GetType().Name, primary);
                failures.Attempt("initialize-prefix.deinitialize", Deinitialize);
                failures.ThrowIfFailed(); throw;
            }
        }

        protected virtual void OnInitialize()
        {

        }

        public void Deinitialize()
        {
            try { OnDeinitialize(); }
            finally { IsInitialized = false; Panel = null; UIPanelConfig = null; }
        }

        protected virtual void OnDeinitialize()
        {

        }
    }
}
