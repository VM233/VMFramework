using System;
using System.Collections.Generic;

namespace VMFramework.Parameters
{
    public interface IParametersInfoProvider
    {
        public IEnumerable<(string name, Type type)> GetParametersInfo();
    }
}