﻿using System.Collections.Generic;

namespace VMFramework.Procedure
{
    public interface IReadOnlyInitializerManager
    {
        public IReadOnlyList<IInitializer> initializers { get; }
        
        public IReadOnlyDictionary<InitActionHandler, InitializationAction> currentPriorityLeftActions { get; }
        
        public int currentPriority { get; }
        
        public bool isInitializing { get; }
        
        public bool isInitialized { get; }
    }
}