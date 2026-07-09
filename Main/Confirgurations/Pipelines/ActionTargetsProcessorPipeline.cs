using System.Collections.Generic;

namespace VMFramework.Configuration
{
    public class ActionTargetsProcessorPipeline
        : ActionTargetsProcessorPipeline<object, IActionTargetsProcessor<object>>
    {
        
    }
    
    public class ActionTargetsProcessorPipeline<TTarget>
        : ActionTargetsProcessorPipeline<TTarget, IActionTargetsProcessor<TTarget>>
    {
        
    }
    
    public class ActionTargetsProcessorPipeline<TTarget, TProcessor> : ProcessorPipeline<TProcessor>
        where TProcessor : IActionTargetsProcessor<TTarget>
    {
        public void ProcessTargets(IReadOnlyList<TTarget> targets)
        {
            TrySort();
            
            foreach (var (processor, _) in processors)
            {
                if (processor.ProcessTargets(targets) == false)
                {
                    break;
                }
            }
        }
    }
}