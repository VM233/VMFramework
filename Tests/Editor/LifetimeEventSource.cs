using System;

namespace VMFramework.Editor.Tests
{
    public sealed class LifetimeEventSource
    {
        public event Action Changed;
        public void Publish() => Changed?.Invoke();
    }
}
