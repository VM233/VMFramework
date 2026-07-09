namespace VMFramework.Configuration
{
    public interface IRingActionProcessor<TTarget>
    {
        /// <returns>是否继续处理</returns>
        public bool ProcessTarget(TTarget target, out TTarget nextTarget);
    }
}