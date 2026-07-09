using System;
using System.Collections.Generic;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    [Serializable]
    public abstract class GeneralWrapperChooser<TWrapper, TItem>
        : IWrapperChooserConfig<TWrapper, TItem>
    {
        public abstract IChooser<TItem> GenerateNewChooser();

        public abstract TItem GetRandomItem(Random random);

        public virtual void GetAvailableItems(ICollection<TItem> items)
        {
            foreach (var wrapper in GetAvailableWrappers())
            {
                foreach (var value in GetAvailableValuesFromWrapper(wrapper))
                {
                    items.Add(value);
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