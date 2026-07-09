using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.GameEvents
{
    public partial class GameEventManager
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsEnabled(string id)
        {
            var gameEvent = GetGameEventStrictly(id);
            
            var isEnabled = gameEvent.IsEnabled.GetValue();
            
            return isEnabled;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable(string id, IToken token)
        {
            var gameEvent = GetGameEventStrictly(id);
            
            gameEvent.IsEnabled.RemoveToken(token);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable(string id, IToken token)
        {
            var gameEvent = GetGameEventStrictly(id);
            
            gameEvent.IsEnabled.AddToken(token);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enable<TEnumerable>(TEnumerable ids, IToken token) where TEnumerable : IEnumerable<string>
        {
            if (ids == null)
            {
                return;
            }
            
            foreach (var id in ids)
            {
                Enable(id, token);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Disable<TEnumerable>(TEnumerable ids, IToken token) where TEnumerable : IEnumerable<string>
        {
            if (ids == null)
            {
                return;
            }
            
            foreach (var id in ids)
            {
                Disable(id, token);
            }
        }
    }
}