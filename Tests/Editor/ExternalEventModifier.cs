using VMFramework.UI;

namespace VMFramework.Editor.Tests
{
    public sealed class ExternalEventModifier : PanelModifier
    {
        public LifetimeEventSource Source { get; set; }
        public int Received { get; private set; }
        public int Deinitializations { get; private set; }
        protected override void OnInitialize() => Source.Changed += OnChanged;
        protected override void OnDeinitialize()
        {
            Source.Changed -= OnChanged;
            Deinitializations++;
        }
        private void OnChanged() => Received++;
    }
}
