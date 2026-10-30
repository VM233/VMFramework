using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.GameLogicArchitecture
{
    [UnityEngine.Scripting.RequireImplementors]
    public partial interface IGameItem
        : IGamePrefabIDOwner, INameOwner, IReadOnlyDestructible, ICreatablePoolItem<string>, IGameTagsOwner,
            IStateClonerOwner
    {

    }
}
