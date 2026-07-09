#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMFramework.GameLogicArchitecture.Editor
{
    public static class GamePrefabProviderQueryTools
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<IGamePrefabsProvider> GetProviders(Type gamePrefabType)
        {
            foreach (var gamePrefabWrapper in GamePrefabWrapperQueryTools.GetGamePrefabWrappers(gamePrefabType))
            {
                yield return gamePrefabWrapper;
            }

            if (gamePrefabType.IsDerivedFromAny(new[] { typeof(IController), typeof(Component) }, false))
            {
                foreach (var gamePrefabBehaviour in gamePrefabType.FindAssetsOfType())
                {
                    if (gamePrefabBehaviour is IGamePrefabsProvider gamePrefabProvider)
                    {
                        yield return gamePrefabProvider;
                    }
                }
            }
        }
    }
}
#endif