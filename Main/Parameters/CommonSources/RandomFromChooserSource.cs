using System.Collections.Generic;
using VMFramework.Core;

namespace VMFramework.Parameters
{
    public struct RandomFromChooserSource<TValue> : IParameterSource<TValue>, ICollectionParametersSource<TValue>
    {
        public IChooser<TValue> chooser;

        public RandomFromChooserSource(IChooser<TValue> chooser)
        {
            this.chooser = chooser;
        }

        public bool TryGetValue(ref TValue value)
        {
            value = chooser.GetRandomItem();
            return true;
        }

        public bool TryGetValues(ICollection<TValue> collection)
        {
            collection.Add(chooser.GetRandomItem());
            return true;
        }
    }
}