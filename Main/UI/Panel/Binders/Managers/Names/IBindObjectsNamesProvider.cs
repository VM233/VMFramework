using System.Collections.Generic;

namespace VMFramework.UI
{
    public interface IBindObjectsNamesProvider
    {
        public void GetBindObjectsNames(ICollection<string> names, ICollection<string> singleModeNames);
    }
}