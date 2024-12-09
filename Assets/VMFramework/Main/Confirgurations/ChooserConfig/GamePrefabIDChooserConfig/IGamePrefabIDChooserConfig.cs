using System.Collections.Generic;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Configuration
{
    public interface IGamePrefabIDChooserConfig<TGamePrefab>
        : IWrapperChooserConfig<GamePrefabIDConfig<TGamePrefab>, string>, IIDCollectionOwner
        where TGamePrefab : IGamePrefab
    {
        void IIDCollectionOwner.GetIDs(ICollection<string> ids)
        {
            ids.AddRange(GetAvailableValues());
        }
    }
}