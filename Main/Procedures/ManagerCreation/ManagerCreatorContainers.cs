using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Procedure
{
    public static class ManagerCreatorContainers
    {
        public const string CONTAINER_NAME = "^Core";
        
        public static Transform ManagerContainer { get; private set; }
        
        private static readonly Dictionary<string, Transform> managerTypeContainers = new();

        public static IReadOnlyDictionary<string, Transform> ManagerTypeContainers =>
            managerTypeContainers;
        
        public static void Init()
        {
            managerTypeContainers.Clear();

            var managerContainerGameObject = CONTAINER_NAME.FindOrCreateGameObject();
            
            managerContainerGameObject.AssertIsNotNull(nameof(managerContainerGameObject));
            
            ManagerContainer = managerContainerGameObject.transform;
            
            ManagerContainer.SetAsFirstSibling();
            
            foreach (var managerType in Enum.GetValues(typeof(ManagerType)).Cast<ManagerType>())
            {
                GetOrCreateManagerTypeContainer(managerType.ToString());
            }
        }
        
        public static Transform GetOrCreateManagerTypeContainer(string managerTypeName)
        {
            if (managerTypeContainers.TryGetValue(managerTypeName, out var managerTypeContainer))
            {
                return managerTypeContainer;
            }
            
            var managerTypeContainerGameObject =
                managerTypeName.FindOrCreateGameObject(ManagerContainer);

            managerTypeContainerGameObject.AssertIsNotNull(nameof(managerTypeContainerGameObject));
                
            managerTypeContainers.Add(managerTypeName, managerTypeContainerGameObject.transform);
            
            return managerTypeContainerGameObject.transform;
        }

        public static IEnumerable<Transform> GetOtherManagerTypeContainers(string managerTypeName)
        {
            return managerTypeContainers.Values.Where(t => t.name != managerTypeName);
        }
        
        public static IEnumerable<Transform> GetAllManagerTypeContainers()
        {
            return managerTypeContainers.Values;
        }
    }
}