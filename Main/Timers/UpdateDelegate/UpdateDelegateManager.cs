using System;
using System.Collections.Generic;
using VMFramework.Procedure;

namespace VMFramework.Timers
{
    [ManagerCreationProvider(ManagerType.TimerCore)]
    public class UpdateDelegateManager : ManagerBehaviour<UpdateDelegateManager>
    {
        public event Action OnFixedUpdateEvent;
        public event Action OnUpdateEvent;
        public event Action OnLateUpdateEvent;
        public event Action OnGUIEvent;

        public event Action OnNextUpdate
        {
            add => nextUpdateActions.Add(value);
            remove => nextUpdateActions.Remove(value);
        }

        protected readonly HashSet<Action> nextUpdateActions = new();
        protected readonly List<Action> nextUpdateActionsTemp = new();

        protected override void Awake()
        {
            base.Awake();

            OnFixedUpdateEvent = null;
            OnUpdateEvent = null;
            OnLateUpdateEvent = null;
            OnGUIEvent = null;

            nextUpdateActions.Clear();
            nextUpdateActionsTemp.Clear();
        }

        protected virtual void FixedUpdate()
        {
            OnFixedUpdateEvent?.Invoke();
        }

        protected virtual void Update()
        {
            OnUpdateEvent?.Invoke();

            if (nextUpdateActions.Count > 0)
            {
                nextUpdateActionsTemp.Clear();
                nextUpdateActionsTemp.AddRange(nextUpdateActions);
                nextUpdateActions.Clear();

                foreach (var action in nextUpdateActionsTemp)
                {
                    action.Invoke();
                }
            }
        }

        protected virtual void LateUpdate()
        {
            OnLateUpdateEvent?.Invoke();
        }

        protected virtual void OnGUI()
        {
            OnGUIEvent?.Invoke();
        }
    }
}