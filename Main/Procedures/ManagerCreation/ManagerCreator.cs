using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif
using VMFramework.Core;

namespace VMFramework.Procedure
{
    public static class ManagerCreator
    {
        public const string SetupMarkerName = "VMFramework.ManagerCreation.Setup";
        public const string DiscoveryMarkerName = "VMFramework.ManagerCreation.Discover";
        public const string SelectionMarkerName = "VMFramework.ManagerCreation.Select";
        public const string RetirementMarkerName = "VMFramework.ManagerCreation.Retire";
        public const string AdoptionMarkerName = "VMFramework.ManagerCreation.Adopt";
        private static readonly ProfilerMarker SetupMarker = new(SetupMarkerName);
        private static readonly ProfilerMarker DiscoveryMarker = new(DiscoveryMarkerName);
        private static readonly ProfilerMarker SelectionMarker = new(SelectionMarkerName);
        private static readonly ProfilerMarker RetirementMarker = new(RetirementMarkerName);
        private static readonly ProfilerMarker AdoptionMarker = new(AdoptionMarkerName);

        private static readonly HashSet<Type> abstractManagerTypes = new();

        private static readonly HashSet<Type> interfaceManagerTypes = new();

        private static readonly HashSet<Type> managerTypes = new();

        private static readonly List<IManagerBehaviour> managers = new();

        public static IReadOnlyCollection<Type> AbstractManagerTypes => abstractManagerTypes;

        public static IReadOnlyCollection<Type> InterfaceManagerTypes => interfaceManagerTypes;

        public static IReadOnlyCollection<Type> ManagerTypes => managerTypes;

        public static IReadOnlyList<IManagerBehaviour> Managers => managers;

        public static async UniTask CreateManagersAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            UniTask<IReadOnlyList<Type>> discovery;
            using (DiscoveryMarker.Auto())
                discovery = InitializationTypeDiscovery.FindAllAsync(typeof(IManagerBehaviour), true, cancellationToken);
            var managerClassTypes = await discovery;
            cancellationToken.ThrowIfCancellationRequested();
            CreateManagers(managerClassTypes);
        }

#if UNITY_EDITOR
        public static void CreateManagers()
        {
            Type[] managerClassTypes;
            using (DiscoveryMarker.Auto())
                managerClassTypes = typeof(IManagerBehaviour).GetDerivedClasses(true, false).ToArray();
            CreateManagers(managerClassTypes);
        }
#endif

        private static void CreateManagers(IReadOnlyList<Type> managerClassTypes)
        {
            using (SetupMarker.Auto())
            {
                ManagerCreatorContainers.Init();

                var eventCoreContainer =
                    ManagerCreatorContainers.GetOrCreateManagerTypeContainer(ManagerType.EventCore.ToString());

                eventCoreContainer.GetOrAddComponent<EventSystem>();
                if (eventCoreContainer.TryGetComponent(out BaseInputModule _) == false)
                {
#if ENABLE_INPUT_SYSTEM
                    eventCoreContainer.AddComponent<InputSystemUIInputModule>();
#else
                    eventCoreContainer.AddComponent<StandaloneInputModule>();
#endif
                }
            }

            abstractManagerTypes.Clear();
            interfaceManagerTypes.Clear();
            managerTypes.Clear();
            managers.Clear();

            var validManagerClassTypes = new Dictionary<Type, ManagerCreationProviderAttribute>();
            var invalidManagerClassTypes = new Dictionary<Type, ManagerCreationProviderAttribute>();

            foreach (var managerClassType in managerClassTypes)
            {
                using (SelectionMarker.Auto())
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
            }

            using (RetirementMarker.Auto())
            {
                foreach (var (managerClassType, providerAttribute) in invalidManagerClassTypes)
                {
                    var managerTypeName = providerAttribute.ManagerTypeName;

                    var container = ManagerCreatorContainers.GetOrCreateManagerTypeContainer(managerTypeName);

                    container.RemoveFirstComponentImmediate(managerClassType);
                }
            }

            using (AdoptionMarker.Auto())
            {
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
            }

            managerTypes.UnionWith(validManagerClassTypes.Keys);
        }
    }
}
