namespace VMFramework.Configuration
{
    public abstract class TypedActionProcessor<TTypedTarget> : TypedActionProcessor<TTypedTarget, object>
    {

    }

    public abstract class TypedActionProcessor<TTypedTarget, TTarget> : IActionProcessor<TTarget>
    {
        public bool ProcessTarget(TTarget target)
        {
            if (target is not TTypedTarget typedTarget)
            {
                return true;
            }

            ProcessTypedTarget(typedTarget);
            return true;
        }

        protected abstract void ProcessTypedTarget(TTypedTarget typedTarget);
    }
}