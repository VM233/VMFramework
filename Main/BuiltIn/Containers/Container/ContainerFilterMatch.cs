using System;

namespace VMFramework.Containers
{
    public struct ContainerFilterMatch
    {
        public string intention;
        public Func<int?, IContainerItem, string, bool> matchFunc;

        public ContainerFilterMatch(string intention, Func<int?, IContainerItem, string, bool> matchFunc)
        {
            this.intention = intention;
            this.matchFunc = matchFunc;
        }
        
        public bool IsMatchFilter(int index, IContainerItem item)
        {
            return matchFunc(index, item, intention);
        }
    }
}