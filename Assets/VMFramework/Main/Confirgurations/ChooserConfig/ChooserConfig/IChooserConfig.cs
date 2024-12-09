using System;
using System.Collections;
using System.Collections.Generic;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public interface IChooserConfig : IConfig, IChooser, IChooserGenerator
    {
        void IChooser.ResetChooser()
        {
            // No need to reset the chooser since this is a configuration.
        }
    }

    public interface IChooserConfig<out TItem> : IChooserConfig, IChooser<TItem>, IChooserGenerator<TItem>
    {
        
    }
}