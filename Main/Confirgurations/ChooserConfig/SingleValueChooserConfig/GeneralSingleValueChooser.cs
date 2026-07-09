using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    [Serializable]
    public abstract partial class GeneralSingleValueChooser<TWrapper, TItem>
        : GeneralWrapperChooser<TWrapper, TItem>
    {
        [HideLabel]
        public TWrapper value;

        protected GeneralSingleValueChooser()
        {
            value = default;
        }

        protected GeneralSingleValueChooser(TWrapper valueWrapper)
        {
            value = valueWrapper;
        }

        public override IChooser<TItem> GenerateNewChooser()
        {
            return new SingleValueChooser<TItem>(UnboxWrapper(value));
        }

        public override TItem GetRandomItem(Random random)
        {
            return UnboxWrapper(value);
        }

        public sealed override IEnumerable<TWrapper> GetAvailableWrappers()
        {
            yield return value;
        }

        public sealed override void SetAvailableValues(Func<TWrapper, TWrapper> setter)
        {
            value = setter(value);
        }

        public override string ToString()
        {
            if (value is IEnumerable enumerable)
            {
                return enumerable.Cast<object>().Join(", ");
            }

            return WrapperToString(value);
        }

        public static implicit operator TWrapper(GeneralSingleValueChooser<TWrapper, TItem> config)
        {
            return config.value;
        }

        public static implicit operator TItem(GeneralSingleValueChooser<TWrapper, TItem> config)
        {
            return config.UnboxWrapper(config.value);
        }
    }
}