using System.Collections;
using System.Collections.Generic;

namespace VMFramework.Containers
{
    public partial class Container
    {
        public IEnumerator<IContainerItem> GetEnumerator()
        {
            return validItemsLookup.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}