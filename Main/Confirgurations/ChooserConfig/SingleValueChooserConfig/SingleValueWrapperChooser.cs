using System;

namespace VMFramework.Configuration
{
    [Serializable]
    public class SingleValueWrapperChooser<TWrapper, TItem> : GeneralSingleValueChooser<TWrapper, TItem>
        where TWrapper : IChooserWrapper<TItem>
    {
        protected override TItem UnboxWrapper(TWrapper wrapper)
        {
            return wrapper.UnboxWrapper();
        }
    }
}