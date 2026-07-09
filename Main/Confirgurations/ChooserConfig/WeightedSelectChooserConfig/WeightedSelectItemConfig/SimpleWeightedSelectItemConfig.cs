using System;
using Sirenix.OdinInspector;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    [Serializable]
    public class SimpleWeightedSelectItemConfig<T> : BaseConfig, IWeightedSelectItem<T>
    {
        [HideLabel]
        public T value;

        [MinValue(0)]
        public int ratio = 1;

        T IWeightedSelectItem<T>.Value => value;

        float IWeightedSelectItem.Weight => ratio;

        public override void CheckSettings()
        {
            base.CheckSettings();

            if (value is ICheckableConfig checkable)
            {
                checkable.CheckSettings();
            }
        }

        protected override void OnInit()
        {
            base.OnInit();

            if (value is IInitializableConfig initializable)
            {
                initializable.Init();
            }
        }
    }
}