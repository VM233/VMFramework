#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace VMFramework.Editor
{
    public sealed class TempConfirmContainer : SerializedScriptableObject
    {
        [field: HideLabel]
        [field: SerializeField]
        public object obj { get; private set; }
        
        private Action onConfirm;
        
        [HideInInspector]
        [SerializeField]
        private OdinEditorWindow window;

        [Button(ButtonSizes.Medium)]
        private void Confirm()
        {
            onConfirm?.Invoke();
            window.Close();
        }

        public void Init(object obj, Action onConfirm, OdinEditorWindow window)
        {
            this.obj = obj;
            this.onConfirm = onConfirm;
            this.window = window;
        }
    }
}
#endif