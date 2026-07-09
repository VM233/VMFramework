using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.OdinExtensions;

namespace VMFramework.GameLogicArchitecture
{
    [HideDuplicateReferenceBox]
    [HideReferenceObjectPicker]
    [PreviewComposite]
    public abstract partial class GameItem : IGameItem, IStateCloner
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
            OnGet();
        }

        void ICreatablePoolItem<string>.OnCreate(string id)
        {
            GamePrefab = GamePrefabManager.GetGamePrefabStrictly(id);

            OnCreate();
            OnGet();
        }

        void IPoolItem.OnReturn()
        {
            OnReturn();
        }

        void IPoolItem.OnClear()
        {
            OnReturn();
            OnClear();
        }

        protected virtual void OnGet()
        {
            IsDestroyed = false;
        }

        protected virtual void OnCreate()
        {

        }

        protected virtual void OnReturn()
        {
            OnDestructed?.Invoke(this);
            IsDestroyed = true;
        }

        protected virtual void OnClear()
        {

        }

        #endregion

        public virtual void CloneFrom(IStateCloner cloner, StateCloneHint hint)
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
    }
}