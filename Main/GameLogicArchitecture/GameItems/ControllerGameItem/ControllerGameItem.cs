using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameEvents;
#if FISHNET
using System.Runtime.CompilerServices;
using FishNet;
using FishNet.Managing.Client;
using FishNet.Managing.Server;
using VMFramework.Network;
#endif

namespace VMFramework.GameLogicArchitecture
{
    public class ControllerGameItem : MonoBehaviour, IControllerGameItem
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

        void IPoolItem.OnReturn() => RetirePoolItem(false);

        void IPoolItem.OnClear() => RetirePoolItem(true);

        private void RetirePoolItem(bool clear)
        {
            bool retireLease = !clear || !IsDestroyed;
            IsDestroyed = true;
            var failures = new GameItemCleanupFailures(this, clear ? "pool.clear" : "pool.return");
            List<IReturnEventProvider.ReturnHandler> callbacks = null;
            try
            {
                if (retireLease)
                {
                    try
                    {
                        callbacks = ListPool<IReturnEventProvider.ReturnHandler>.Default.Get();
                        callbacks.Clear();
                        foreach (var combined in returnEvents.GetCombinedCallbacks())
                            foreach (var callback in combined.GetInvocationList())
                                callbacks.Add((IReturnEventProvider.ReturnHandler)callback);
                    }
                    catch (Exception failure) { failures.Capture(failure); }

                    if (callbacks != null)
                        foreach (var callback in callbacks)
                            try { callback(this); }
                            catch (Exception failure) { failures.Capture(failure); }

                    try { OnReturn(); }
                    catch (Exception failure) { failures.Capture(failure); }
                }
                if (clear)
                    try { OnClear(); }
                    catch (Exception failure) { failures.Capture(failure); }
            }
            finally
            {
                if (callbacks != null)
                {
                    try { callbacks.Clear(); }
                    catch (Exception failure) { failures.Capture(failure); }
                    try { callbacks.ReturnToDefaultPool(); }
                    catch (Exception failure) { failures.Capture(failure); }
                }
            }
            failures.ThrowIfFailed();
        }

        protected virtual void OnDestroy() => IsDestroyed = true;

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
            var failures = new GameItemCleanupFailures(this, "return.notification");
            var subscribers = OnDestructed;
            if (subscribers != null)
                foreach (Action<IReadOnlyDestructible> subscriber in subscribers.GetInvocationList())
                    try { subscriber(this); }
                    catch (Exception failure) { failures.Capture(failure); }
            try { gameObject.SetActive(false); }
            catch (Exception failure) { failures.Capture(failure); }
            failures.ThrowIfFailed();
        }

        protected virtual void OnClear()
        {

        }

        #endregion

#if UNITY_EDITOR
        [TitleGroup(ComponentNames.RUNTIME)]
        [ShowInInspector, DisplayAsString]
        private string _id => GamePrefab?.id;
#endif

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

#if FISHNET
        private bool isInitialized;
        private IUUIDOwner uuidOwner;
        private INetworkSerializer networkSerializer;

        public IUUIDOwner UUIDOwner
        {
            get
            {
                if (isInitialized == false)
                {
                    Initialize();
                }

                return uuidOwner;
            }
        }

        public INetworkSerializer NetworkSerializer
        {
            get
            {
                if (isInitialized == false)
                {
                    Initialize();
                }

                return networkSerializer;
            }
        }

        private void Initialize()
        {
            uuidOwner = GetComponent<IUUIDOwner>();
            networkSerializer = GetComponent<INetworkSerializer>();
            isInitialized = true;
        }

        #region Properties

        public static bool IsServer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => InstanceFinder.IsServerStarted;
        }

        public static bool IsClient
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => InstanceFinder.IsClientStarted;
        }

        public static bool IsHost
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => InstanceFinder.IsHostStarted;
        }

        public static bool IsServerOnly
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => InstanceFinder.IsServerOnlyStarted;
        }

        public static bool IsClientOnly
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => InstanceFinder.IsClientOnlyStarted;
        }

        public static ServerManager ServerManager => InstanceFinder.ServerManager;
        public static ClientManager ClientManager => InstanceFinder.ClientManager;

        #endregion
#endif
    }
}
