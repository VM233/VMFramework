namespace VMFramework.Configuration
{
    public class ActionProcessorPipeline : ActionProcessorPipeline<object, IActionProcessor<object>>
    {
        
    }
    
    public class ActionProcessorPipeline<TTarget> : ActionProcessorPipeline<TTarget, IActionProcessor<TTarget>>
    {

    }

    public class ActionProcessorPipeline<TTarget, TProcessor> : ProcessorPipeline<TProcessor>
        where TProcessor : IActionProcessor<TTarget>
    {
        public void ProcessTarget(TTarget target)
        {
            TrySort();
            
            foreach (var (processor, _) in processors)
            {
                if (processor.ProcessTarget(target) == false)
                {
                    break;
                }
            }
        }
    }
}