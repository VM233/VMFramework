using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using VMFramework.Core;

namespace VMFramework.Procedure
{
    public static class ManagerCreator
    {
        private static readonly HashSet<Type> abstractManagerTypes = new();

        private static readonly HashSet<Type> interfaceManagerTypes = new();

        private static readonly HashSet<Type> managerTypes = new();

        private static readonly List<IManagerBehaviour> managers = new();

        public static IReadOnlyCollection<Type> AbstractManagerTypes => abstractManagerTypes;

        public static IReadOnlyCollection<Type> InterfaceManagerTypes => interfaceManagerTypes;

        public static IReadOnlyCollection<Type> ManagerTypes => managerTypes;
        
        public static IReadOnlyList<IManagerBehaviour> Managers => managers;

        public static void CreateManagers()
        {
            ManagerCreatorContainers.Init();

            var eventCoreContainer =
                ManagerCreatorContainers.GetOrCreateManagerTypeContainer(ManagerType.EventCore.ToString());

            eventCoreContainer.GetOrAddComponent<EventSystem>();
            if (eventCoreContainer.TryGetComponent(out BaseInputModule _) == false)
            {
                eventCoreContainer.AddComponent<StandaloneInputModule>();
            }

            abstractManagerTypes.Clear();
            interfaceManagerTypes.Clear();
            managerTypes.Clear();
            managers.Clear();

            var validManagerClassTypes = new Dictionary<Type, ManagerCreationProviderAttribute>();
            var invalidManagerClassTypes = new Dictionary<Type, ManagerCreationProviderAttribute>();

            foreach (var managerClassType in typeof(IManagerBehaviour).GetDerivedClasses(true, false))
            {
                if (managerClassType.TryGetAttribute<ManagerCreationProviderAttribute>(true,
                        out var providerAttribute) == false)
                {
                    continue;
                }

                if (managerClassType.IsAbstract)
                {
                    abstractManagerTypes.Add(managerClassType);
                    continue;
                }

                if (managerClassType.IsInterface)
                {
                    interfaceManagerTypes.Add(managerClassType);
                    continue;
                }

                if (managerClassType.IsDerivedFrom<Component>(false, false) == false)
                {
                    UnityEngine.Debug.LogWarning($"{managerClassType} is not derived from {nameof(Component)}");
                    continue;
                }

                var isParentOrChild = false;

                foreach (var (validManagerType, validProviderAttribute) in validManagerClassTypes.ToList())
                {
                    if (managerClassType.IsSubclassOf(validManagerType))
                    {
                        validManagerClassTypes.Remove(validManagerType);
                        invalidManagerClassTypes.Add(validManagerType, validProviderAttribute);
                        validManagerClassTypes.Add(managerClassType, providerAttribute);

                        isParentOrChild = true;
                        break;
                    }

                    if (validManagerType.IsSubclassOf(managerClassType))
                    {
                        invalidManagerClassTypes.Add(managerClassType, providerAttribute);

                        isParentOrChild = true;
                        break;
                    }
                }

                if (isParentOrChild == false)
                {
                    validManagerClassTypes.Add(managerClassType, providerAttribute);
                }
            }

            foreach (var (managerClassType, providerAttribute) in invalidManagerClassTypes)
            {
                var managerTypeName = providerAttribute.ManagerTypeName;

                var container = ManagerCreatorContainers.GetOrCreateManagerTypeContainer(managerTypeName);

                container.RemoveFirstComponentImmediate(managerClassType);
            }

            foreach (var (managerClassType, providerAttribute) in validManagerClassTypes)
            {
                var managerTypeName = providerAttribute.ManagerTypeName;

                var container = ManagerCreatorContainers.GetOrCreateManagerTypeContainer(managerTypeName);
                
                var component = container.GetOrAddComponent(managerClassType);

                if (component is not IManagerBehaviour managerBehaviour)
                {
                    throw new Exception($"{managerClassType} does not implement {nameof(IManagerBehaviour)}");
                }
                
                managers.Add(managerBehaviour);
                
                foreach (var otherContainer in ManagerCreatorContainers.GetOtherManagerTypeContainers(managerTypeName))
                {
                    otherContainer.RemoveAllComponentsImmediate(managerClassType);
                }
            }

            managerTypes.UnionWith(validManagerClassTypes.Keys);
        }
    }
}