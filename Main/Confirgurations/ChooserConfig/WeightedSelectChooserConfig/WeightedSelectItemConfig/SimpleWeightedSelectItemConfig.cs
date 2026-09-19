using System;
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    [Serializable]
    public class SimpleWeightedSelectItemConfig<T> : IWeightedSelectItem<T>, ICheckableConfig,
        IInitializableConfig
    {
        [field: NonSerialized]
        public bool InitDone { get; private set; }

        [HideLabel]
        public T value;

        [MinValue(0)]
        public int ratio = 1;

        T IWeightedSelectItem<T>.Value => value;

        float IWeightedSelectItem.Weight => ratio;

        public void CheckSettings()
        {
            if (value is ICheckableConfig checkable)
            {
                checkable.CheckSettings();
            }
        }

        public void Init()
        {
            if (value is IInitializableConfig initializable)
            {
                initializable.Init();
            }

            InitDone = true;
        }
    }
}
