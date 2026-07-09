using System.Collections.Generic;
using VMFramework.Core.Pools;

namespace VMFramework.Configuration
{
    public abstract class TypedFuncTargetsProcessor<TTypedTarget, TResult>
        : TypedFuncTargetsProcessor<TTypedTarget, object, TResult>
    {
        
    }
    
    public abstract class TypedFuncTargetsProcessor<TTypedTarget, TTarget, TResult>
        : IFuncTargetsProcessor<TTarget, TResult>
    {
        public void ProcessTargets(IReadOnlyCollection<TTarget> targets, ICollection<TResult> results)
        {
            var typedTargets = ListPool<TTypedTarget>.Shared.Get();
            typedTargets.Clear();

            foreach (TTarget target in targets)
            {
                if (target is TTypedTarget typedTarget)
                {
                    typedTargets.Add(typedTarget);
                }
            }

            if (typedTargets.Count == 0)
            {
                goto RETURN;
            }

            ProcessTypedTargets(typedTargets, results);

            RETURN:
            typedTargets.ReturnToSharedPool();
        }
        
        protected abstract void ProcessTypedTargets(IReadOnlyList<TTypedTarget> targets,
            ICollection<TResult> results);
    }
}