using System;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    [Serializable]
#if UNITY_EDITOR
    [HideDuplicateReferenceBox]
    [HideReferenceObjectPicker]
    [OnInspectorInit("@((IInspectorConfig)$value)?.OnInspectorInit()")]
#endif
    public class WeightedSelectItemConfig<T> : IWeightedSelectItem<T>, ICloneable
#if UNITY_EDITOR
        , IInspectorConfig
#endif
    {
        [SerializeReference]
        public T value;

        [LabelWidth(30), HorizontalGroup]
        [MinValue(0)]
        public int ratio;

        [LabelText("Probability"), LabelWidth(30), SuffixLabel("%", Overlay = true), HorizontalGroup]
        [DisplayAsString]
        [NonSerialized]
        public float probability;

        [HideLabel, HorizontalGroup]
        [GUIColor("@Color.yellow")]
        [DisplayAsString]
        [NonSerialized]
        public string tag;

        public object Clone()
        {
            return new WeightedSelectItemConfig<T>()
            {
                value = value,
                ratio = ratio,
            };
        }

        T IWeightedSelectItem<T>.Value => value;

        float IWeightedSelectItem.Weight => ratio;

#if UNITY_EDITOR
        void IInspectorConfig.OnInspectorInit()
        {
            ReflectionUtility.TryCreateInstance(ref value);
        }
#endif
    }
}
