namespace VMFramework.Configuration
{
    public abstract class WrapperChooserConfig<TItem> : GeneralWrapperChooser<TItem, TItem>
    {
        protected sealed override TItem UnboxWrapper(TItem wrapper)
        {
            return wrapper;
        }
    }
    
    public abstract class WrapperChooserConfig<TWrapper, TItem> : GeneralWrapperChooser<TWrapper, TItem>
        where TWrapper : IChooserWrapper<TItem>
    {
        protected override TItem UnboxWrapper(TWrapper wrapper)
        {
            return wrapper.UnboxWrapper();
        }
    }
}