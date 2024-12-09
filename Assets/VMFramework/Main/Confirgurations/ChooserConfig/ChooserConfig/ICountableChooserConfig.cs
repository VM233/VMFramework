using System.Collections;
using System.Collections.Generic;

namespace VMFramework.Configuration
{
    public interface ICountableChooserConfig : IChooserConfig
    {
        public IEnumerable GetAvailableObjectValues();
    }
    
    public interface ICountableChooserConfig<out TItem> : ICountableChooserConfig, IChooserConfig<TItem>
    {
        public IEnumerable<TItem> GetAvailableValues();

        IEnumerable ICountableChooserConfig.GetAvailableObjectValues()
        {
            return GetAvailableValues();
        }
    }
}