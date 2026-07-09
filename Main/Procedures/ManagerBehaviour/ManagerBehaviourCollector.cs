using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Procedure
{
    public static class ManagerBehaviourCollector
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyList<IManagerBehaviour> Collect()
        {
            var result = new List<IManagerBehaviour>();

            var allGameObjects = Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include);

            foreach (var go in allGameObjects)
            {
                var behaviours = go.GetComponents<IManagerBehaviour>();

                if (behaviours.Length > 0)
                {
                    result.AddRange(behaviours);
                }
            }

            return result;
        }
    }
}