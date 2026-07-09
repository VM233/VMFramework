using System.Collections.Generic;

namespace VMFramework.Configuration
{
    public interface IActionTargetsProcessor<in TTarget>
    {
        /// <returns>是否继续处理</returns>
        public bool ProcessTargets(IReadOnlyList<TTarget> targets);
    }
}