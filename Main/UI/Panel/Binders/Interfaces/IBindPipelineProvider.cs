using VMFramework.Configuration;

namespace VMFramework.UI
{
    public interface IBindPipelineProvider
    {
        public RingTargetsProcessorPipeline<object> Pipeline { get; }
    }
}