using UnityEngine.Scripting;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    [Preserve]
    public class PrefabProviderCheckProcessor : TypedActionProcessor<IPrefabProvider>, ICheckProcessor
    {
        protected override void ProcessTypedTarget(IPrefabProvider typedTarget)
        {
            if (typedTarget.Prefab == null)
            {
                UnityEngine.Debug.LogWarning($"{nameof(PrefabProviderCheckProcessor)}" +
                                    $"{typedTarget.GetType()} : {typedTarget} " +
                                    $"has no {nameof(typedTarget.Prefab)} assigned.");
            }
        }
    }
}