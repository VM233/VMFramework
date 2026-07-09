using System;
using System.Collections.Generic;

namespace VMFramework.Configuration
{
    public interface IWrapperChooserConfig : ICountableChooserConfig
    {
        
    }

    public interface IWrapperChooserConfig<TWrapper, TItem> : IWrapperChooserConfig, ICountableChooserConfig<TItem>
    {
        public IEnumerable<TWrapper> GetAvailableWrappers();

        public void SetAvailableValues(Func<TWrapper, TWrapper> setter);
    }
}