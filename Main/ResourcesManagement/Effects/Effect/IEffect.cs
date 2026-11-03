using VMFramework.GameLogicArchitecture;

using UnityEngine;

namespace VMFramework.Effects
{
    public interface IEffect : IControllerGameItem, IGameItemDelayedReturnReceiver, IGameItemDelayedReturnDispatcher
    {
        void Stop();

        void SetSpawnTransform(Vector3 position, bool isWorldSpace, Transform parent, bool alignToParent = false);
    }
}
