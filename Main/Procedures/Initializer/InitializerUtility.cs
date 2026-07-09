using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Procedure
{
    public static class InitializerUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<(int order, IList<InitializationAction>)> GetInitializationActions(
            this IList<IInitializer> initializers)
        {
            var initializerActions = new List<InitializationAction>();

            foreach (var initializer in initializers)
            {
                initializer.GetInitializationActions(initializerActions);
            }

            foreach (var actionInfo in initializerActions)
            {
                if (actionInfo.action == null)
                {
                    Debug.LogError($"The action with order : {actionInfo.order} is null." +
                                   $"It's provided by {actionInfo.initializer.GetType()}.");
                }
            }

            var dict = initializerActions.BuildSortedDictionary(initializer => (initializer.order, initializer),
                Comparer<int>.Create((x, y) => x.CompareTo(y)));

            foreach (var (order, listOfActions) in dict)
            {
                yield return (order, listOfActions.ToList());
            }
        }
    }
}