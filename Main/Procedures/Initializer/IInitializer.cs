using System.Collections.Generic;

namespace VMFramework.Procedure
{
    public interface IInitializer
    {
        public bool EnableInitializationDebugLog => false;

        public void GetInitializationActions(ICollection<InitializationAction> actions);
    }
}
