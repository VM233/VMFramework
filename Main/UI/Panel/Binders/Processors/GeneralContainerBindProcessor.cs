using VMFramework.Configuration;
using VMFramework.Core;

namespace VMFramework.UI
{
    public class GeneralContainerBindProcessor : BindProcessorBase
    {
        protected override IFuncTargetsProcessor<object, object> Processor { get; } = new ContainerQueryProcessor();

        protected override int DefaultPriority => PriorityDefines.MEDIUM;
    }
}