namespace VMFramework.Core.Pools
{
    public interface IPoolItem
    {
        /// <summary>
        /// OnGet is called when the item is retrieved from the pool.
        /// </summary>
        public void OnGet()
        {
            
        }

        /// <summary>
        /// OnReturn is called when the item is returned to the pool.
        /// </summary>
        public void OnReturn()
        {
            
        }

        /// <summary>
        /// OnClear is called when the item is cleared from the pool.
        /// </summary>
        public void OnClear()
        {
            
        }
    }
}