using System.Collections.Generic;

namespace VMFramework.Procedure
{
    public interface IReadOnlyInitializerManager
    {
        public IReadOnlyList<IInitializer> Initializers { get; }
        
        public IReadOnlyList<InitializationActionExecution> CurrentOrderExecutions { get; }
        
        public int? CurrentOrder { get; }

        public System.TimeSpan InitializationTimeout { get; }

        public System.Exception LastException { get; }
        
        public bool IsInitializing { get; }
        
        public bool IsInitialized { get; }
    }
}
