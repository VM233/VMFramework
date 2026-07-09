using System.Collections.Generic;

namespace VMFramework.Configuration
{
    public abstract class TypedFuncProcessor<TTypedTarget, TResult> : TypedFuncProcessor<TTypedTarget, object, TResult>
    {

    }

    public abstract class TypedFuncProcessor<TTypedTarget, TTarget, TResult> : IFuncProcessor<TTarget, TResult>
    {
        public void ProcessTarget(TTarget target, ICollection<TResult> results)
        {
            if (target is not TTypedTarget typedTarget)
            {
                return;
            }

            ProcessTypedTarget(typedTarget, results);
        }

        protected abstract void ProcessTypedTarget(TTypedTarget target, ICollection<TResult> results);
    }
}