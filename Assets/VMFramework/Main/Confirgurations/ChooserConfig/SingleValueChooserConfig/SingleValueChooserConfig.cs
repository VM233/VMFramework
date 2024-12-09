using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public partial class SingleValueChooserConfig<TItem> : SingleValueChooserConfig<TItem, TItem> where TItem : new()
    {
        public SingleValueChooserConfig() : base()
        {
            
        }

        public SingleValueChooserConfig(TItem value) : base(value)
        {
            
        }
        
        protected override TItem UnboxWrapper(TItem wrapper)
        {
            return wrapper;
        }
        
        public static implicit operator SingleValueChooserConfig<TItem>(TItem value)
        {
            return new SingleValueChooserConfig<TItem>(value);
        }
    }
    
    public abstract partial class SingleValueChooserConfig<TWrapper, TItem> : WrapperChooserConfig<TWrapper, TItem>
    {
        [HideLabel]
        public TWrapper value;

        protected SingleValueChooserConfig()
        {
            value = default;
        }

        protected SingleValueChooserConfig(TWrapper valueWrapper)
        {
            value = valueWrapper;
        }

        protected override void OnInit()
        {
            base.OnInit();
            
            if (value is IConfig config)
            {
                config.Init();
            }
            else if (value is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    if (item is IConfig itemConfig)
                    {
                        itemConfig.Init();
                    }
                }
            }
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

        public static implicit operator TWrapper(SingleValueChooserConfig<TWrapper, TItem> config)
        {
            return config.value;
        }

        public static implicit operator TItem(SingleValueChooserConfig<TWrapper, TItem> config)
        {
            return config.UnboxWrapper(config.value);
        }
    }
}