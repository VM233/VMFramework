using System;
using System.Collections.Generic;
using VMFramework.Core;
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
            if (GameItemType is { IsAbstract: true })
            {
                Debugger.LogError($"{nameof(GameItemType)} is abstract. " +
                               $"Please override with a concrete type instead of {GameItemType}");
            }
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