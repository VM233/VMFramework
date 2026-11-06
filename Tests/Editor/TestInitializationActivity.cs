using System;
using VMFramework.Procedure;

namespace VMFramework.Tests
{
    internal sealed class TestInitializationActivity : IInitializationActivity
    {
        public bool Active { get; private set; }
        public bool Disposed { get; private set; }
        public event Action<bool> Changed;
        public TestInitializationActivity(bool active) => Active = active;
        public void Set(bool active) { Active = active; Changed?.Invoke(active); }
        public void Dispose() => Disposed = true;
    }
}
