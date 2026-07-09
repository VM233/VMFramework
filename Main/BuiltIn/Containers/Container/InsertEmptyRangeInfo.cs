using System.Collections.Generic;
using VMFramework.Core;

namespace VMFramework.Containers
{
    public struct InsertEmptyRangeInfo
    {
        public RangeInteger? range;
        public ICollection<int> excludedIndices;
        public ContainerFilterMatch filterMatch;

        public InsertEmptyRangeInfo(RangeInteger? range, ICollection<int> excludedIndices,
            ContainerFilterMatch filterMatch)
        {
            this.range = range;
            this.excludedIndices = excludedIndices;
            this.filterMatch = filterMatch;
        }
    }
}