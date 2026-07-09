using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameEvents;

namespace VMFramework.GameLogicArchitecture
{
    public partial class ControllerGameItem : MonoBehaviour, IControllerGameItem
    {
        #region Properties & Fields

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector, HideInEditorMode]
        protected IGamePrefab GamePrefab { get; private set; }
        
        public string id => GamePrefab.id;

        public string Name => GamePrefab.Name;

        public bool IsDebugging => GamePrefab.IsDebugging;
        
        public ICollection<string> GameTags => GamePrefab.GameTags;

        public IStateCloner StateCloner { get; protected set; }

        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector, ReadOnly]
        public bool IsDestroyed { get; private set; } = false;

        public event Action<IReadOnlyDestructible> OnDestructed;

        #endregion

        #region Pool Events

        public IReadOnlyPriorityEvents<IPoolEventProvider.GetHandler> GetEvents => getEvents;

        public IReadOnlyPriorityEvents<IPoolEventProvider.ReturnHandler> ReturnEvents => returnEvents;

        public event IPoolEventProvider.GetHandler OnGetEvent
        {
            add => getEvents.Add(PriorityDefines.MEDIUM, value);
            remove => getEvents.Remove(value);
        }
        public event IPoolEventProvider.ReturnHandler OnReturnEvent
        {
            add => returnEvents.Add(PriorityDefines.MEDIUM, value);
            remove => returnEvents.Remove(value);
        }

        protected readonly PriorityEvents<IPoolEventProvider.GetHandler> getEvents = new();
        protected readonly PriorityEvents<IPoolEventProvider.ReturnHandler> returnEvents = new();

        void IPoolItem.OnGet()
        {
            IsDestroyed = false;
            OnGet();
            foreach (var callback in getEvents.GetCombinedCallbacks())
            {
                callback(this);
            }
        }

        void ICreatablePoolItem<string>.OnCreate(string id)
        {
            GamePrefab = GamePrefabManager.GetGamePrefabStrictly<IGamePrefab>(id);
            
            IsDestroyed = false;
            OnCreate();
            OnGet();
            foreach (var callback in getEvents.GetCombinedCallbacks())
            {
                callback(this);
            }
        }

        void IPoolItem.OnReturn()
        {
            IsDestroyed = true;
            
            var callbacks = returnEvents.GetCombinedCallbacks().ToListDefaultPooled();
            foreach (var callback in callbacks)
            {
                callback(this);
            }
            callbacks.ReturnToDefaultPool();
            OnReturn();
        }

        void IPoolItem.OnClear()
        {
            IsDestroyed = true;
            
            foreach (var callback in returnEvents.GetCombinedCallbacks())
            {
                callback(this);
            }
            OnReturn();
            OnClear();
        }

        protected virtual void OnGet()
        {
            gameObject.SetActive(true);
        }

        protected virtual void OnCreate()
        {
            name = id;
            StateCloner = GetComponent<IStateCloner>();
        }

        protected virtual void OnReturn()
        {
            OnDestructed?.Invoke(this);
            gameObject.SetActive(false);
        }

        protected virtual void OnClear()
        {

        }

        #endregion

        #region To String

        protected virtual void OnGetStringProperties(
            ICollection<(string propertyID, string propertyContent)> collection)
        {

        }

        public override string ToString()
        {
            var list = ListPool<(string propertyID, string propertyContent)>.Shared.Get();
            list.Clear();
            OnGetStringProperties(list);

            var extraString = list.Select(property => property.propertyID + ":" + property.propertyContent).Join(", ");
            list.ReturnToSharedPool();

            return $"[{GetType()}:id:{id},{extraString}]";
        }

        #endregion
    }
}