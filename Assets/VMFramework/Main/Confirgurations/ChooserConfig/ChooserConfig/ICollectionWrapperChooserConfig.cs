﻿namespace VMFramework.Configuration
{
    public interface ICollectionWrapperChooserConfig<TWrapper, out TItem> : IWrapperChooserConfig<TWrapper, TItem>
    {
        public bool ContainsWrapper(TWrapper wrapper);
        
        public void AddWrapper(TWrapper wrapper);
        
        public void RemoveWrapper(TWrapper wrapper);
    }
}