using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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
        }

        protected override void GetInitializationActions(ICollection<InitializationAction> actions)
        {
            base.GetInitializationActions(actions);
            actions.Add(new InitializationAction((int)InitializationOrder.BeforeInitStart - 1, CollectReceivers, this));
        }

        private async UniTask CollectReceivers(CancellationToken cancellationToken)
        {
            var types = await InitializationTypeDiscovery.FindAsync(typeof(IGameItemEventsReceiver), false, cancellationToken);
            foreach (var type in types)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var receiver = (IGameItemEventsReceiver)Activator.CreateInstance(type);
                receivers.Add(receiver);
            }
        }
    }
}
