using System;
using Sirenix.OdinInspector;

namespace VMFramework.Configuration
{
    [Serializable]
    public class RatioLerpConfig<TValue>
    {
        [PropertyRange(0, 1)]
        public float ratio;

        public TValue value;
    }
}