namespace VMFramework.UI
{
    public interface ITooltip : IUIPanel
    {
        public void Open(ITooltipProvider tooltipProvider, IUIPanel source, TooltipOpenInfo info);

        public void Close(ITooltipProvider tooltipProvider);
    }
}