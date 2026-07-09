using System.Collections.Generic;
using VMFramework.Core.Pools;

namespace VMFramework.Configuration
{
    public class RingTargetsProcessorPipeline<TTarget>
        : RingTargetsProcessorPipeline<TTarget, IFuncTargetsProcessor<TTarget, TTarget>>
    {
        
    }
    
    public class RingTargetsProcessorPipeline<TTarget, TProcessor> : ProcessorPipeline<TProcessor>
        where TProcessor : IFuncTargetsProcessor<TTarget, TTarget>
    {
        public void Process(TTarget target, ICollection<TTarget> results)
        {
            TrySort();
            
            var targets = ListPool<TTarget>.Shared.Get();
            targets.Clear();
            var tempResults = ListPool<TTarget>.Shared.Get();
            tempResults.Clear();
            
            targets.Add(target);

            foreach (var (processor, _) in processors)
            {
                processor.ProcessTargets(targets, tempResults);
                targets.Clear();
                targets.AddRange(tempResults);
                tempResults.Clear();
            }

            foreach (var result in targets)
            {
                results.Add(result);
            }
        }
    }
}