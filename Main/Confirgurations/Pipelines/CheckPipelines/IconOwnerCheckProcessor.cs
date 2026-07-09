using System;
using Sirenix.OdinInspector;
using UnityEngine.Scripting;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Configuration
{
    [Preserve]
    public class IconOwnerCheckProcessor : TypedActionProcessor<IIconOwner>, ICheckProcessor
    {
        protected override void ProcessTypedTarget(IIconOwner typedTarget)
        {
            var iconProperty = typeof(IIconOwner).GetProperty(nameof(IIconOwner.Icon));

            if (iconProperty == null)
            {
                return;
            }

            var iconPropertyGetter = iconProperty.GetGetMethod();

            var map = typedTarget.GetType().GetInterfaceMap(typeof(IIconOwner));
            var index = Array.IndexOf(map.InterfaceMethods, iconPropertyGetter);

            if (index >= 0)
            {
                var targetMethod = map.TargetMethods[index];

                bool hasAttribute = Attribute.IsDefined(targetMethod, typeof(RequiredAttribute));

                if (hasAttribute)
                {
                    if (typedTarget.Icon == null)
                    {
                        UnityEngine.Debug.LogWarning($"[{nameof(IconOwnerCheckProcessor)}]" +
                                            $"The {nameof(typedTarget.Icon)} of {typedTarget} is not set.");
                    }
                }
            }
        }
    }
}