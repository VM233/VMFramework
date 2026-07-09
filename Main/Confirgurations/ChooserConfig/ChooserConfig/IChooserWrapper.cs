namespace VMFramework.Configuration
{
    public interface IChooserWrapper<out TItem>
    {
        public TItem UnboxWrapper();
    }
}