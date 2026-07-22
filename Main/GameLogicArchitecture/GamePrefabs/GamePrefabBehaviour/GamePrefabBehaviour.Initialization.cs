using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GamePrefabBehaviour
    {
        protected virtual void GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.Init, OnInitInternal, this));
        }
        
        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            GetInitializationActions(actions);
        }

        public virtual void CheckSettings()
        {
            
        }

        private UniTask OnInitInternal(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            OnInit();
            return UniTask.CompletedTask;
        }

        protected virtual void OnInit()
        {
            
        }
    }
}
