using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.OdinExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    [Conditional("UNITY_EDITOR")]
    public sealed class PrefabComponentTypeAttribute : Attribute, IGamePrefabIDFilterAttribute
    {
        public Type[] ComponentTypes;

        public bool isAllRequired = false;

        public PrefabComponentTypeAttribute(params Type[] componentTypes)
        {
            ComponentTypes = componentTypes;
        }

        public IEnumerable<IGamePrefab> GetGamePrefabs()
        {
            foreach (var prefabProvider in GamePrefabManager.GetAllGamePrefabs<IPrefabProvider>())
            {
                var prefab = prefabProvider.Prefab;

                if (prefab == null)
                {
                    continue;
                }

                if (isAllRequired)
                {
                    if (ComponentTypes.All(componentType => prefab.GetComponent(componentType)))
                    {
                        yield return (IGamePrefab)prefabProvider;
                    }
                }
                else
                {
                    if (ComponentTypes.Any(componentType => prefab.GetComponent(componentType)))
                    {
                        yield return (IGamePrefab)prefabProvider;
                    }
                }
            }
        }
    }
}