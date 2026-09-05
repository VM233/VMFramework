using System;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.Configuration
{
    [Serializable]
    public struct TypeFilter : IFilter
    {
        public bool isMultiple;

        [HideIf(nameof(isMultiple))]
        public SerializableType type;

        [ShowIf(nameof(isMultiple))]
        [IsNotNullOrEmpty]
        public SerializableType[] types;

        [ShowIf(nameof(isMultiple))]
        public bool isAll;

        public bool inversed;

        public bool IsMatch(object obj)
        {
            if (isMultiple)
            {
                if (types is { Length: > 0 })
                {
                    var objType = obj.GetType();
                    if (isAll)
                    {
                        bool isTargetType = true;
                        foreach (var type in types)
                        {
                            if (objType.IsDerivedFrom(type, true) == false)
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
                            if (objType.IsDerivedFrom(type, true))
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
                if (type?.Value != null)
                {
                    var isTargetType = obj.GetType().IsDerivedFrom(type, true);
                    return isTargetType ^ inversed;
                }
            }

            return true;
        }
    }
}
