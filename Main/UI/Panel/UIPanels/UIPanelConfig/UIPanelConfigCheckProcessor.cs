using UnityEngine.Scripting;
using VMFramework.Configuration;
using VMFramework.Core;

namespace VMFramework.UI
{
    [Preserve]
    public class UIPanelConfigCheckProcessor : TypedActionProcessor<IUIPanelConfig>, ICheckProcessor
    {
        protected override void ProcessTypedTarget(IUIPanelConfig typedTarget)
        {
            if (typedTarget.IsUnique)
            {
                if (typedTarget.GameItemPrewarmCount > 0)
                {
                    UnityEngine.Debug.LogError($"[{nameof(UIPanelConfigCheckProcessor)}]" +
                                      $"{typedTarget} is marked as unique but has a " +
                                      $"{nameof(typedTarget.GameItemPrewarmCount)} of {typedTarget.GameItemPrewarmCount}." +
                                      $"It should be set to 0.");
                }
            }
        }
    }
}