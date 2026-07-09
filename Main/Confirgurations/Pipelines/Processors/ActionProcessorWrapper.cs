using System;
using System.Diagnostics.CodeAnalysis;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public readonly struct ActionProcessorWrapper<TTarget> : IActionProcessor<TTarget>
    {
        public readonly Func<TTarget, bool> func;

        public ActionProcessorWrapper([DisallowNull] Func<TTarget, bool> func)
        {
            func.AssertIsNotNull(nameof(func));
            this.func = func;
        }

        public bool ProcessTarget(TTarget target)
        {
            return func(target);
        }
    }
}