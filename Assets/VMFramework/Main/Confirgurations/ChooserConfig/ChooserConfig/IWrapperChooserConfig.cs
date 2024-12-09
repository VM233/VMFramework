using System;
using System.Collections;
using System.Collections.Generic;

namespace VMFramework.Configuration
{
    public interface IWrapperChooserConfig : ICountableChooserConfig
    {
        
    }

    public interface IWrapperChooserConfig<TWrapper, out TItem> : IWrapperChooserConfig, ICountableChooserConfig<TItem>
    {
        public IEnumerable<TWrapper> GetAvailableWrappers();

        public void SetAvailableValues(Func<TWrapper, TWrapper> setter);
    }
}