namespace VMFramework.UI
{
    public interface ITooltip
    {
        public bool Open(object target, IUIPanel source, TooltipOpenInfo info);

        public bool Close(object target);
    }
}