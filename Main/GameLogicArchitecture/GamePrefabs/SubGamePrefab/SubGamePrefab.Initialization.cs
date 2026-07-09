using System;
using System.Collections.Generic;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    public partial class SubGamePrefab
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

        private void OnInitInternal(Action onDone)
        {
            OnInit();
            onDone();
        }

        protected virtual void OnInit()
        {
            
        }
    }
}