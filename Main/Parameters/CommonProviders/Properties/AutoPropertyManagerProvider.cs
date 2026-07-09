using UnityEngine;
using VMFramework.Containers;
using VMFramework.Properties;

namespace VMFramework.Parameters
{
    [ParameterDefine(PROPERTY_MANAGER_PARAMETER, typeof(PropertyManager))]
    public class AutoPropertyManagerProvider : MonoBehaviour, IParameterProvider
    {
        public enum Mode
        {
            Owner,
            ContainerItemOwner
        }

        public const string PROPERTY_MANAGER_PARAMETER = "Property Manager";

        public Mode mode = Mode.Owner;

        protected IPropertyManagerOwner owner;
        protected IContainerItem containerItem;

        protected virtual void Awake()
        {
            if (mode is Mode.Owner)
            {
                owner = GetComponentInParent<IPropertyManagerOwner>();
            }
            else if (mode is Mode.ContainerItemOwner)
            {
                containerItem = GetComponentInParent<IContainerItem>();
            }
        }

        public bool TryGetParameter<TValue>(string key, ref TValue value)
        {
            if (key == PROPERTY_MANAGER_PARAMETER)
            {
                if (mode is Mode.Owner)
                {
                    if (owner.PropertyManager is TValue validValue)
                    {
                        value = validValue;
                        return true;
                    }
                }
                else if (mode is Mode.ContainerItemOwner)
                {
                    if (containerItem.Owner.GetValue() is IPropertyManagerOwner
                        {
                            PropertyManager: TValue validValue
                        })
                    {
                        value = validValue;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}