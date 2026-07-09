using System;
using System.Collections.Generic;

namespace VMFramework.Parameters
{
    public interface ICollectionParametersInfoProvider
    {
        public IEnumerable<(string name, Type type)> GetCollectionParametersInfo();
    }
}