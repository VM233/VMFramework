namespace VMFramework.Configuration
{
    public interface IActionProcessor<in TTarget>
    {
        /// <returns>是否继续处理</returns>
        public bool ProcessTarget(TTarget target);
    }
}