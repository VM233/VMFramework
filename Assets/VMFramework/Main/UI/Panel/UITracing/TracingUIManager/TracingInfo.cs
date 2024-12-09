namespace VMFramework.UI
{
    internal class TracingInfo
    {
        public TracingConfig config {get; private set; }
        
        public int tracingCount = 1;

        public void SetConfig(TracingConfig config)
        {
            this.config = config;
        }
    }
}