using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Procedure
{
    public class InitializerManager : IReadOnlyInitializerManager
    {
        private readonly List<IInitializer> initializers = new();

        private readonly Dictionary<InitActionHandler, InitializationAction> currentPriorityLeftActions = new();

        #region Properties

        [ShowInInspector]
        public IReadOnlyList<IInitializer> Initializers => initializers;

        [ShowInInspector]
        public IReadOnlyDictionary<InitActionHandler, InitializationAction> CurrentPriorityLeftActions =>
            currentPriorityLeftActions;

        [ShowInInspector]
        public int CurrentPriority { get; private set; }

        [ShowInInspector]
        public bool IsInitializing { get; private set; }

        [ShowInInspector]
        public bool IsInitialized { get; private set; }

        #endregion

        public void Set(IEnumerable<IInitializer> initializers)
        {
            if (IsInitializing)
            {
                throw new InvalidOperationException($"Cannot reset {nameof(InitializerManager)}, " +
                                                    $"while it is still initializing.");
            }

            this.initializers.Clear();
            this.initializers.AddRange(initializers);

            IsInitialized = false;
        }

        public async UniTask Initialize()
        {
            if (IsInitializing)
            {
                throw new InvalidOperationException($"Cannot initialize {nameof(InitializerManager)}, " +
                                                    $"while it is still initializing.");
            }

            IsInitialized = false;
            IsInitializing = true;

            var initializersName = initializers.Select(initializer => initializer.GetType().ToString());

            var initializersNameWithTag = initializersName.Select(name => name.ColorTag("green")).ToList();

            if (initializersNameWithTag.Count > 0)
            {
                var names = initializersNameWithTag.Join(", ");

                UnityEngine.Debug.Log($"Initializer: {names} Started!");
            }

            foreach (var (priority, listOfActions) in initializers.GetInitializationActions())
            {
                CurrentPriority = priority;
                currentPriorityLeftActions.Clear();

                foreach (var actionInfo in listOfActions)
                {
                    if (currentPriorityLeftActions.TryAdd(actionInfo.action, actionInfo))
                    {
                        continue;
                    }

                    Debug.LogError($"Duplicate initialization action: {actionInfo.action} detected in " +
                                   $"priority:{priority} which is provided by {actionInfo.initializer.GetType()}" +
                                   $"while it's already provided by " +
                                   $"{currentPriorityLeftActions[actionInfo.action].initializer.GetType()}");
                }

                foreach (var actionInfo in listOfActions)
                {
                    if (actionInfo.initializer.EnableInitializationDebugLog)
                    {
                        var initializerName = actionInfo.initializer.GetType().ToString();

                        if (actionInfo.initializer is IIDOwner<string> idOwner)
                        {
                            initializerName += $": {idOwner.id}";
                        }
                        
                        UnityEngine.Debug.Log($"Initializing {actionInfo.action.Method.Name} of {initializerName}");
                    }

                    actionInfo.action(() => currentPriorityLeftActions.Remove(actionInfo.action));
                }

                await UniTask.WaitUntil(() => currentPriorityLeftActions.Count == 0);
            }

            IsInitializing = false;
            IsInitialized = true;
        }
    }
}