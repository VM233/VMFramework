using Sirenix.OdinInspector;

namespace VMFramework.UI
{
    internal class TracingInfo
    {
        [ShowInInspector]
        public TracingConfig Config { get; private set; }

        public int tracingCount = 0;

        public void Initialize(TracingConfig config)
        {
            Config = config;
            tracingCount = 0;
        }
    }
}