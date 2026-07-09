using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Procedure;

namespace VMFramework.Tools
{
    [ManagerCreationProvider(ManagerType.OtherCore)]
    public class ContainerTransform : ManagerBehaviour<ContainerTransform>
    {
        private static readonly Dictionary<string, Transform> containers = new();

        protected override void Awake()
        {
            base.Awake();
            
            containers.Clear();
        }

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