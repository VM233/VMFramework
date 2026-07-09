namespace VMFramework.UI
{
    public interface ISlotFilter
    {
        /// <summary>
        /// 如果返回true，表示slot满足过滤条件。
        /// 如果返回false，表示slot不满足过滤条件。
        /// 如果返回null，表示不参与过滤。
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public bool? IsMatch(SlotVisualElement slot);
    }
}