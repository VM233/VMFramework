using VMFramework.UI;
using UnityEngine;

namespace VMFramework.Editor.Tests
{
    public sealed class ExternalEventModifier : IPanelModifier
    {
        private readonly LifetimeEventSource source;
        public GameObject gameObject { get; }
        public Transform transform => gameObject.transform;
        public IUIPanel Panel { get; private set; }
        public int InitializePriority => 0;
        public bool IsInitialized { get; private set; }
        public int Received { get; private set; }
        public int Deinitializations { get; private set; }
        public ExternalEventModifier(GameObject host, LifetimeEventSource source)
        {
            gameObject = host;
            this.source = source;
        }
        public void Initialize(IUIPanel panel, IUIPanelConfig config)
        {
            Panel = panel;
            source.Changed += OnChanged;
            IsInitialized = true;
        }
        public void Deinitialize()
        {
            source.Changed -= OnChanged;
            Deinitializations++;
            IsInitialized = false;
        }
        private void OnChanged() => Received++;
        public T GetComponent<T>() => gameObject.GetComponent<T>();
        public T[] GetComponents<T>() => gameObject.GetComponents<T>();
        public bool TryGetComponent<T>(out T component) => gameObject.TryGetComponent(out component);
        public T[] GetComponentsInChildren<T>() => gameObject.GetComponentsInChildren<T>();
        public T GetComponentInChildren<T>() => gameObject.GetComponentInChildren<T>();
    }
}
