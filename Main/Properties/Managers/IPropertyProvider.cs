using System.Collections.Generic;

namespace VMFramework.Properties
{
    public interface IPropertyProvider
    {
        public void GetProperties(ICollection<IReadOnlyProperty> properties);
    }
}