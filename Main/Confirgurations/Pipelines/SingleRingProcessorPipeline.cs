namespace VMFramework.Configuration
{
    public class SingleRingProcessorPipeline<TTarget>
        : SingleRingProcessorPipeline<TTarget, IRingActionProcessor<TTarget>>
    {

    }

    public class SingleRingProcessorPipeline<TTarget, TProcessor> : ProcessorPipeline<TProcessor>
        where TProcessor : IRingActionProcessor<TTarget>
    {
        public void Process(TTarget target, out TTarget result)
        {
            TrySort();

            result = target;
            foreach (var (processor, _) in processors)
            {
                if (processor.ProcessTarget(result, out result) == false)
                {
                    return;
                }
            }
        }
    }
}