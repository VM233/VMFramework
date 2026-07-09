using System.Collections.Generic;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public interface IChoosersGenerator<T>
    {
        public void GenerateChoosers(ICollection<IChooser<T>> choosers);
    }
}