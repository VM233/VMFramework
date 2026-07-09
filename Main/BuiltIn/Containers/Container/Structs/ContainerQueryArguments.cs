using VMFramework.Core;

namespace VMFramework.Containers
{
    public struct ContainerQueryArguments
    {
        public string intention;

        public RangeInteger? range;

        public ContainerQueryArguments(string intention, RangeInteger? range = null)
        {
            this.intention = intention;
            this.range = range;
        }
    }
}