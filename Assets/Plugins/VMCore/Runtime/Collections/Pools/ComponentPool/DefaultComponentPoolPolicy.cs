using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core.Pools
{
    public sealed class DefaultComponentPoolPolicy<TComponent> : ComponentPoolPolicy<TComponent>
        where TComponent : Component
    {
        private readonly Func<TComponent> _createFunc;
        
        public DefaultComponentPoolPolicy(Func<TComponent> createFunc)
        {
            _createFunc = createFunc;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TComponent Create() => _createFunc();
    }
}