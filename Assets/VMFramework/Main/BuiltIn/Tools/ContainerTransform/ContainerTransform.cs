using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Tools
{
    public static class ContainerTransform
    {
        private static readonly Dictionary<string, Transform> containers = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Transform Get(string containerName)
        {
            if (containers.TryGetValue(containerName, out Transform container))
            {
                return container;
            }
            
            container = new GameObject(containerName).transform;
            containers.Add(containerName, container);
            return container;
        }
    }
}