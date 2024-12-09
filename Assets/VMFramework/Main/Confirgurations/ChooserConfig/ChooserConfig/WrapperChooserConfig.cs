using System;
using System.Collections.Generic;
using System.Linq;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    public abstract class WrapperChooserConfig<TItem> : WrapperChooserConfig<TItem, TItem>
    {
        protected sealed override TItem UnboxWrapper(TItem wrapper)
        {
            return wrapper;
        }
    }
    
    public abstract class WrapperChooserConfig<TWrapper, TItem> : ChooserConfig<TItem>, IWrapperChooserConfig<TWrapper, TItem>
    {
        public virtual IEnumerable<TItem> GetAvailableValues()
        {
            foreach (var wrapper in GetAvailableWrappers())
            {
                foreach (var value in GetAvailableValuesFromWrapper(wrapper))
                {
                    yield return value;
                }
            }
        }

        public abstract IEnumerable<TWrapper> GetAvailableWrappers();

        public abstract void SetAvailableValues(Func<TWrapper, TWrapper> setter);

        protected virtual string WrapperToString(TWrapper value)
        {
            if (value is IEnumerable<object> enumerable)
            {
                return $"[{enumerable.Join(";")}]";
            }
            return value?.ToString();
        }

        protected abstract TItem UnboxWrapper(TWrapper wrapper);

        protected virtual IEnumerable<TItem> GetAvailableValuesFromWrapper(TWrapper wrapper)
        {
            yield return UnboxWrapper(wrapper);
        }
    }
}