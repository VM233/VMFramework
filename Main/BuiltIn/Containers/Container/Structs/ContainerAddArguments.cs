using VMFramework.Core;

namespace VMFramework.Containers
{
    public struct ContainerAddArguments
    {
        public string intention;

        /// <summary>
        /// 添加的槽位范围。
        /// </summary>
        public RangeInteger? slotRange;

        public int? preferredCount;

        public ContainerAddArguments(RangeInteger? slotRange)
        {
            this.intention = null;
            this.slotRange = slotRange;
            this.preferredCount = null;
        }

        public ContainerAddArguments(int slotIndex)
        {
            this.intention = null;
            this.slotRange = new RangeInteger(slotIndex, slotIndex);
            this.preferredCount = null;
        }

        public ContainerAddArguments(string intention, RangeInteger? slotRange = null)
        {
            this.intention = intention;
            this.slotRange = slotRange;
            this.preferredCount = null;
        }
        
        public ContainerAddArguments(string intention, int slotIndex)
        {
            this.intention = intention;
            this.slotRange = new RangeInteger(slotIndex, slotIndex);
            this.preferredCount = null;
        }
    }
}