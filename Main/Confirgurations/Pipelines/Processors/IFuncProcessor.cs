using System.Collections.Generic;

namespace VMFramework.Configuration
{
    public interface IFuncProcessor<in TTarget, TResult>
    {
        public void ProcessTarget(TTarget target, ICollection<TResult> results);
    }
}