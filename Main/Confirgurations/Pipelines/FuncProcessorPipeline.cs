using System.Collections.Generic;

namespace VMFramework.Configuration
{
    public class FuncProcessorPipeline<TResult>
        : FuncProcessorPipeline<object, TResult, IFuncProcessor<object, TResult>>
    {

    }

    public class FuncProcessorPipeline<TTarget, TResult>
        : FuncProcessorPipeline<TTarget, TResult, IFuncProcessor<TTarget, TResult>>
    {

    }

    public class FuncProcessorPipeline<TTarget, TResult, TProcessor> : ProcessorPipeline<TProcessor>
        where TProcessor : IFuncProcessor<TTarget, TResult>
    {
        public bool ProcessTarget(TTarget target, ICollection<TResult> results)
        {
            TrySort();
            
            var initialCount = results.Count;
            
            foreach (var (processor, _) in processors)
            {
                processor.ProcessTarget(target, results);
            }
            
            return results.Count > initialCount;
        }
    }
}