using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;

namespace VMFramework.GameLogicArchitecture
{
    public partial class ControllerGameItem : MonoBehaviour, IGameItem, IController
    {
        #region Properties & Fields

        [ShowInInspector, HideInEditorMode]
        protected IGamePrefab GamePrefab { get; private set; }
        
        public string id => GamePrefab.id;

        public string Name => GamePrefab.Name;

        public bool IsDebugging => GamePrefab.IsDebugging;
        
        public ICollection<string> GameTags => GamePrefab.GameTags;

        [ShowInInspector, ReadOnly]
        public bool IsDestroyed { get; private set; } = false;

        #endregion

        #region Clone

        public virtual void CloneTo(IGameItem other)
        {

        }

        #endregion

        #region Pool Events

        void IPoolItem.OnGet()
        {
            IsDestroyed = false;
            OnGet();
        }

        void ICreatablePoolItem<string>.OnCreate(string id)
        {
            GamePrefab = GamePrefabManager.GetGamePrefabStrictly<IGamePrefab>(id);
            
            IsDestroyed = false;
            OnCreate();
            OnGet();
        }

        void IPoolItem.OnReturn()
        {
            OnReturn();
            IsDestroyed = true;
        }

        void IPoolItem.OnClear()
        {
            OnReturn();
            IsDestroyed = true;
            OnClear();
        }

        protected virtual void OnGet()
        {
            gameObject.SetActive(true);
        }

        protected virtual void OnCreate()
        {

        }

        protected virtual void OnReturn()
        {
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
            OnGetStringProperties(list);

            var extraString = list.Select(property => property.propertyID + ":" + property.propertyContent).Join(", ");
            list.ReturnToSharedPool();

            return $"[{GetType()}:id:{id},{extraString}]";
        }

        #endregion
    }
}