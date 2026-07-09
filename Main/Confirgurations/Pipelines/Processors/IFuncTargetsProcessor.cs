using System.Collections.Generic;

namespace VMFramework.Configuration
{
    public interface IFuncTargetsProcessor<in TTarget, TResult>
    {
        public void ProcessTargets(IReadOnlyCollection<TTarget> targets, ICollection<TResult> results);
    }
}