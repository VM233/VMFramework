using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    public class CircularSelectChooserConfig<TItem>
        : CircularSelectChooserConfig<TItem, TItem>, ICircularSelectChooserConfig<TItem>
    {
        protected sealed override TItem UnboxWrapper(TItem wrapper)
        {
            return wrapper;
        }
    }
    
    public abstract partial class CircularSelectChooserConfig<TWrapper, TItem> : WrapperChooserConfig<TWrapper, TItem>, ICircularSelectChooserConfig<TWrapper, TItem>
    {
        [LabelText("从第几个开始循环"), SuffixLabel("Begin with 0")]
        [MinValue(0)]
        [JsonProperty]
        public int startCircularIndex = 0;

        [LabelText("乒乓循环")]
        [PropertyTooltip("循环到底后，从后往前遍历")]
#if UNITY_EDITOR
        [ShowIf(nameof(ShowPingPongOption))]
#endif
        [JsonProperty]
        public bool pingPong = false;

#if UNITY_EDITOR
        [ListDrawerSettings(CustomAddFunction = nameof(AddItemGUI), ShowFoldout = true)]
        [OnValueChanged(nameof(OnItemsChangedGUI), true)]
        [OnCollectionChanged(nameof(OnItemsChangedGUI))]
#endif
        [IsNotNullOrEmpty]
        [JsonProperty]
        public List<CircularSelectItemConfig<TWrapper>> items = new();

        private CircularSelectChooser<TItem> chooser;

        protected override void OnInit()
        {
            base.OnInit();
            
            foreach (var item in items)
            {
                if (item.value is IConfig config)
                {
                    config.Init();
                }
                else if (item.value is IEnumerable enumerable)
                {
                    foreach (var obj in enumerable)
                    {
                        if (obj is IConfig configObj)
                        {
                            configObj.Init();
                        }
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private CircularSelectChooser<TItem> GenerateThisChooser()
        {
            return new CircularSelectChooser<TItem>(
                items.Select(item => new CircularSelectItem<TItem>(UnboxWrapper(item.value), item.times)),
                pingPong, startCircularIndex);
        }

        public override IChooser<TItem> GenerateNewChooser()
        {
            return GenerateThisChooser();
        }

        public override TItem GetRandomItem(Random random)
        {
            chooser ??= GenerateThisChooser();
            return chooser.GetRandomItem(random);
        }

        public override IEnumerable<TWrapper> GetAvailableWrappers()
        {
            return items.Select(item => item.value);
        }

        public override void SetAvailableValues(Func<TWrapper, TWrapper> setter)
        {
            foreach (var item in items)
            {
                item.value = setter(item.value);
            }
        }

        public bool ContainsWrapper(TWrapper wrapper)
        {
            return items.Any(item => item.value.Equals(wrapper));
        }

        public void AddWrapper(TWrapper wrapper)
        {
            items.Add(new CircularSelectItemConfig<TWrapper> { value = wrapper });
#if UNITY_EDITOR
            OnItemsChangedGUI();
#endif
        }

        public void RemoveWrapper(TWrapper wrapper)
        {
            items.RemoveAll(item => item.value.Equals(wrapper));
#if UNITY_EDITOR
            OnItemsChangedGUI();
#endif
        }

        public override string ToString()
        {
            var content = ", ".Join(items.Select(item =>
            {
                if (item.times > 1)
                {
                    return $"{WrapperToString(item.value)}:{item.times} times";
                }

                return WrapperToString(item.value);
            }));

            if (pingPong)
            {
                content += " PingPong";
            }

            return content;
        }
    }
}