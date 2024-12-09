using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core.Pools
{
    public sealed class CustomComponentPoolPolicy<TComponent> : ComponentPoolPolicy<TComponent>
        where TComponent : Component
    {
        private readonly Func<TComponent, TComponent> _preGetFunc;
        private readonly Func<TComponent> _createFunc;
        private readonly Func<TComponent, bool> _returnFunc;
        private readonly Action<TComponent> _clearFunc;

        public CustomComponentPoolPolicy(Func<TComponent> createFunc, Func<TComponent, TComponent> preGetFunc = null,
            Func<TComponent, bool> returnFunc = null, Action<TComponent> clearFunc = null)
        {
            createFunc.AssertIsNotNull(nameof(createFunc));
            _createFunc = createFunc;
            _preGetFunc = preGetFunc ?? base.PreGet;
            _returnFunc = returnFunc?? base.Return;
            _clearFunc = clearFunc?? base.Clear;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TComponent Create() => _createFunc();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override TComponent PreGet(TComponent item) => _preGetFunc(item);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Return(TComponent item) => _returnFunc(item);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Clear(TComponent item) => _clearFunc(item);
    }
}