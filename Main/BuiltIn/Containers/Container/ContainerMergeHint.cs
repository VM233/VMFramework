namespace VMFramework.Containers
{
    public struct ContainerMergeHint
    {
        public static readonly ContainerMergeHint Default = new(false);
        
        /// <summary>
        /// 当物品不可拆分时，是否强制合并
        /// </summary>
        public bool forceMergeWhenNonSplittable;

        public ContainerMergeHint(bool forceMergeWhenNonSplittable)
        {
            this.forceMergeWhenNonSplittable = forceMergeWhenNonSplittable;
        }
    }
}