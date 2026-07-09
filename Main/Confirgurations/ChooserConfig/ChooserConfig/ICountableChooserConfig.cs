using VMFramework.Core;

namespace VMFramework.Configuration
{
    public interface ICountableChooserConfig : IChooser, IAvailableItemsProvider
    {
        
    }

    public interface ICountableChooserConfig<TItem>
        : ICountableChooserConfig, IChooser<TItem>, IAvailableItemsProvider<TItem>
    {

    }
}