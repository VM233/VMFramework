using System;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.GameEvents
{
    public interface IReadOnlyGameEvent : IGameItem, ITokenEnabledOwner
    {
        public void AddCallback(Delegate callback, int priority);
        
        public void RemoveCallback(Delegate callback);
    }
}