using System;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    public struct ComponentTypeFilter : IFilter
    {
        public bool isMultiple;

        [HideIf(nameof(isMultiple))]
        public Type type;

        [ShowIf(nameof(isMultiple))]
        [IsNotNullOrEmpty]
        public Type[] types;

        [ShowIf(nameof(isMultiple))]
        public bool isAll;

        public bool inversed;

        public bool IsMatch(object obj)
        {
            if (obj.TryAsGameObject(out var gameObject) == false)
            {
                return true;
            }

            if (isMultiple)
            {
                if (types is { Length: > 0 })
                {
                    if (isAll)
                    {
                        bool isTargetType = true;
                        foreach (var type in types)
                        {
                            if (gameObject.GetComponent(type) == null)
                            {
                                isTargetType = false;
                                break;
                            }
                        }

                        return isTargetType ^ inversed;
                    }
                    else
                    {
                        bool isTargetType = false;
                        foreach (var type in types)
                        {
                            if (gameObject.GetComponent(type) != null)
                            {
                                isTargetType = true;
                                break;
                            }
                        }

                        return isTargetType ^ inversed;
                    }
                }
            }
            else
            {
                if (type != null)
                {
                    var isTargetType = gameObject.GetComponent(type) != null;
                    return isTargetType ^ inversed;
                }
            }

            return true;
        }
    }
}