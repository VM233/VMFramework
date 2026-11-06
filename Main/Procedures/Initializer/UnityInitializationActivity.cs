using System;
using UnityEngine;

namespace VMFramework.Procedure
{
    internal sealed class UnityInitializationActivity : IInitializationActivity
    {
        public bool Active { get; private set; }
        public event Action<bool> Changed;
        private readonly bool batchMode;
#if UNITY_EDITOR
        private readonly bool editing;
#endif

        public UnityInitializationActivity()
        {
            batchMode = Application.isBatchMode;
            if (batchMode) { Active = true; return; }
#if UNITY_EDITOR
            editing = !Application.isPlaying;
            if (editing)
            {
                Active = UnityEditor.EditorApplication.isFocused;
                UnityEditor.EditorApplication.focusChanged += FocusChanged;
                return;
            }
#endif
            Active = Application.isFocused;
            Application.focusChanged += FocusChanged;
        }

        private void FocusChanged(bool active)
        {
            Active = active;
            Changed?.Invoke(active);
        }

        public void Dispose()
        {
            if (batchMode) return;
#if UNITY_EDITOR
            if (editing)
            {
                UnityEditor.EditorApplication.focusChanged -= FocusChanged;
                return;
            }
#endif
            Application.focusChanged -= FocusChanged;
        }
    }
}
