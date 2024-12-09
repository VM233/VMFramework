namespace VMFramework.Core.Pools
{
    public sealed class ArrayPoolPolicy<TItem> : IPoolPolicy<TItem[]>
    {
        private readonly int arrayLength;

        public ArrayPoolPolicy(int arrayLength)
        {
            this.arrayLength = arrayLength;
        }

        public TItem[] PreGet(TItem[] item) => item;

        public TItem[] Create() => new TItem[arrayLength];

        public bool Return(TItem[] item)
        {
            if (item == null)
            {
                return false;
            }

            if (item.Length != arrayLength)
            {
                return false;
            }

            return true;
        }

        public void Clear(TItem[] item)
        {
        }
    }
}