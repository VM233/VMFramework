using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.GameEvents
{
    public partial class GameEventManager
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEnabled(string id)
        {
            var gameEvent = GetGameEventStrictly(id);
            
            return gameEvent.IsEnabled;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Enable(string id, IToken token)
        {
            var gameEvent = GetGameEventStrictly(id);
            
            gameEvent.Enable(token);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Disable(string id, IToken token)
        {
            var gameEvent = GetGameEventStrictly(id);
            
            gameEvent.Disable(token);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Enable<TEnumerable>(TEnumerable ids, IToken token) where TEnumerable : IEnumerable<string>
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
        public static void Disable<TEnumerable>(TEnumerable ids, IToken token) where TEnumerable : IEnumerable<string>
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