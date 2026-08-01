using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            ManagerContainer = GetOrCreateManagerContainer();
            
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

            managerTypeContainer = GetOrCreateDirectChild(ManagerContainer, managerTypeName);
            managerTypeContainers.Add(managerTypeName, managerTypeContainer);

            return managerTypeContainer;
        }

        private static Transform GetOrCreateManagerContainer()
        {
            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.IsValid() == false || activeScene.isLoaded == false)
            {
                throw new InvalidOperationException(
                    "Manager containers require a valid, loaded active scene.");
            }

            var matchingRoots = activeScene.GetRootGameObjects()
                .Where(gameObject => gameObject.name == CONTAINER_NAME)
                .ToArray();

            if (matchingRoots.Length > 1)
            {
                throw new InvalidOperationException(
                    $"The active scene contains multiple root '{CONTAINER_NAME}' objects.");
            }

            return matchingRoots.Length == 1
                ? matchingRoots[0].transform
                : new GameObject(CONTAINER_NAME).transform;
        }

        private static Transform GetOrCreateDirectChild(Transform parent, string childName)
        {
            var matchingChildren = parent.Cast<Transform>()
                .Where(child => child.name == childName)
                .ToArray();

            if (matchingChildren.Length > 1)
            {
                throw new InvalidOperationException(
                    $"Manager container '{parent.name}' contains multiple direct '{childName}' children.");
            }

            if (matchingChildren.Length == 1)
            {
                return matchingChildren[0];
            }

            var childObject = new GameObject(childName);
            childObject.transform.SetParent(parent, false);
            return childObject.transform;
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
