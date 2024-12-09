using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.GameLogicArchitecture
{
    public partial interface IGameItem : IIDOwner<string>, INameOwner, IDestructible, ICreatablePoolItem<string>, IGameTagsOwner
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CloneTo(IGameItem other);

        void IDestructible.Destruct()
        {
            GameItemManager.Return(this);
        }
    }
}