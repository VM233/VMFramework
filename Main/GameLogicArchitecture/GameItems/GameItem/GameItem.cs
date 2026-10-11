using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.OdinExtensions;
#if FISHNET
using FishNet.CodeGenerating;
using FishNet.Serializing;
using VMFramework.Network;
#endif

namespace VMFramework.GameLogicArchitecture
{
    [HideDuplicateReferenceBox]
    [HideReferenceObjectPicker]
    [PreviewComposite]
    public abstract class GameItem : IGameItem, IStateCloner
#if FISHNET
        , INetworkSerializer
#endif
    {
        #region Properties & Fields

        [ShowInInspector]
        protected IGamePrefab GamePrefab { get; private set; }

        [ShowInInspector, DisplayAsString]
        public string id => GamePrefab.id;

        public string Name => GamePrefab.Name;

        [ShowInInspector]
        public bool IsDebugging => GamePrefab.IsDebugging;

        public ICollection<string> GameTags => GamePrefab.GameTags;

        public IStateCloner StateCloner => this;

        [ShowInInspector]
        public bool IsDestroyed { get; private set; } = false;

        public event Action<IReadOnlyDestructible> OnDestructed;

        #endregion

        #region Pool Events

        void IPoolItem.OnGet()
        {
            IsDestroyed = false;
            OnGet();
        }

        void ICreatablePoolItem<string>.OnCreate(string id)
        {
            GamePrefab = GamePrefabManager.GetGamePrefabStrictly(id);
            IsDestroyed = false;
            OnCreate();
            OnGet();
        }

        void IPoolItem.OnReturn()
        {
            IsDestroyed = true;
            OnReturn();
        }

        void IPoolItem.OnClear()
        {
            bool retireLease = !IsDestroyed;
            IsDestroyed = true;
            var failures = new GameItemCleanupFailures(this, "managed.clear");
            if (retireLease)
                try { OnReturn(); }
                catch (Exception failure) { failures.Capture(failure); }
            try { OnClear(); }
            catch (Exception failure) { failures.Capture(failure); }
            failures.ThrowIfFailed();
        }

        protected virtual void OnGet()
        {

        }

        protected virtual void OnCreate()
        {

        }

        protected virtual void OnReturn()
        {
            var failures = new GameItemCleanupFailures(this, "return.notification");
            var subscribers = OnDestructed;
            if (subscribers != null)
                foreach (Action<IReadOnlyDestructible> subscriber in subscribers.GetInvocationList())
                    try { subscriber(this); }
                    catch (Exception failure) { failures.Capture(failure); }
            failures.ThrowIfFailed();
        }

        protected virtual void OnClear()
        {

        }

        #endregion

        public virtual void CloneFrom(IStateCloner cloner, StateCloneContext context)
        {

        }

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
        public virtual IUUIDOwner UUIDOwner => null;

        public virtual INetworkSerializer NetworkSerializer => this;

        #region Network Serialization

        /// <summary>
        /// 在网络上如何传输，当在此实例被写进byte流时调用
        /// </summary>
        /// <param name="writer"></param>
        [NotSerializer]
        protected virtual void OnWrite(Writer writer)
        {

        }

        /// <summary>
        /// 在网络上如何传输，当在此实例被从byte流中读出时调用
        /// </summary>
        /// <param name="reader"></param>
        [NotSerializer]
        protected virtual void OnRead(Reader reader)
        {

        }

        void INetworkSerializer.WriteFishnet(Writer writer)
        {
            OnWrite(writer);
        }

        void INetworkSerializer.ReadFishnet(Reader reader)
        {
            OnRead(reader);
        }

        #endregion
#endif
    }
}
