using System.Collections.Generic;

namespace VMFramework.Configuration
{
    public class FuncTargetsProcessorPipeline<TResult>
        : FuncTargetsProcessorPipeline<object, TResult, IFuncTargetsProcessor<object, TResult>>
    {

    }

    public class FuncTargetsProcessorPipeline<TTarget, TResult>
        : FuncTargetsProcessorPipeline<TTarget, TResult, IFuncTargetsProcessor<TTarget, TResult>>
    {

    }

    public class FuncTargetsProcessorPipeline<TTarget, TResult, TProcessor> : ProcessorPipeline<TProcessor>
        where TProcessor : IFuncTargetsProcessor<TTarget, TResult>
    {
        public bool ProcessTargets(IReadOnlyCollection<TTarget> targets, ICollection<TResult> results)
        {
            TrySort();

            var initialCount = results.Count;

            foreach (var (processor, _) in processors)
            {
                processor.ProcessTargets(targets, results);
            }

            return results.Count > initialCount;
        }
    }
}