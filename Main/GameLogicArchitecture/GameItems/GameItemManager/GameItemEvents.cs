using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    [ManagerCreationProvider(ManagerType.GameItemCore)]
    public class GameItemEvents : ManagerBehaviour<GameItemEvents>
    {
        [ShowInInspector]
        protected readonly HashSet<IGameItemEventsReceiver> receivers = new();
        
        public event Action<IGameItem> OnGameItemCreated
        {
            add
            {
                foreach (var receiver in receivers)
                {
                    receiver.OnGameItemCreated += value;
                }
            }
            remove
            {
                foreach (var receiver in receivers)
                {
                    receiver.OnGameItemCreated -= value;
                }
            }
        }
        
        public event Action<IGameItem> OnGameItemDestroyed
        {
            add
            {
                foreach (var receiver in receivers)
                {
                    receiver.OnGameItemDestroyed += value;
                }
            }
            remove
            {
                foreach (var receiver in receivers)
                {
                    receiver.OnGameItemDestroyed -= value;
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
         
            receivers.Clear();
            foreach (var type in typeof(IGameItemEventsReceiver).GetDerivedInstantiableClasses(false))
            {
                var receiver = (IGameItemEventsReceiver)Activator.CreateInstance(type);
                receivers.Add(receiver);
            }
        }
    }
}