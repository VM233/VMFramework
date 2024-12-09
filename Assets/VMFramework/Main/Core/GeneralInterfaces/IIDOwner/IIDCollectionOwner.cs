using System.Collections.Generic;

namespace VMFramework.Core
{
    public interface IIDCollectionOwner
    {
        public void GetIDs(ICollection<string> ids);
    }
}