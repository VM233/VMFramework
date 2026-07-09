using UnityEngine.Scripting;
using VMFramework.Configuration;
using VMFramework.Core;

namespace VMFramework.GameLogicArchitecture
{
    [Preserve]
    public class GamePrefabCheckProcessor : TypedActionProcessor<IGamePrefab>, ICheckProcessor
    {
        protected override void ProcessTypedTarget(IGamePrefab typedTarget)
        {
            if (typedTarget is not IPrefabProvider)
            {
                if (typedTarget.GameItemType is { IsAbstract: true })
                {
                    UnityEngine.Debug.LogError($"[{nameof(GamePrefabCheckProcessor)}]" +
                                      $"{nameof(typedTarget.GameItemType)} of {typedTarget} is abstract. " +
                                      $"Please override with a concrete type instead of {typedTarget.GameItemType}");
                }
            }
        }
    }
}