namespace VMFramework.Configuration
{
    public interface IWeightedSelectChooserConfig<TItem> : IWeightedSelectChooserConfig<TItem, TItem>
    {

    }

    public interface IWeightedSelectChooserConfig<TWrapper, TItem> : ICollectionWrapperChooserConfig<TWrapper, TItem>
    {

    }
}