using System.Collections.Generic;

namespace VMFramework.Procedure
{
    public interface IReadOnlyInitializerManager
    {
        public IReadOnlyList<IInitializer> Initializers { get; }
        
        public IReadOnlyDictionary<InitActionHandler, InitializationAction> CurrentPriorityLeftActions { get; }
        
        public int CurrentPriority { get; }
        
        public bool IsInitializing { get; }
        
        public bool IsInitialized { get; }
    }
}