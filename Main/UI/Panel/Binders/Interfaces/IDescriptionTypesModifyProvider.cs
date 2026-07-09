using System.Collections.Generic;

namespace VMFramework.UI
{
    public interface IDescriptionTypesModifyProvider
    {
        public delegate void GetHandler(object target, IList<string> descriptionTypes);
        
        public event GetHandler OnModifyDescriptionTypes;
    }
}