using System;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.Properties;

namespace VMFramework.GameEvents
{
    public interface IReadOnlyGameEvent : IGameItem, IToken
    {
        public ITokenProperty<IToken> IsEnabled { get; }

        public bool IsValid();
        
        public void AddCallback(Delegate callback, int priority);
        
        public void RemoveCallback(Delegate callback);
    }
}