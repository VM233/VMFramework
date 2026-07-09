#if FISHNET
using System;
using VMFramework.Core;
using VMFramework.Procedure;

namespace VMFramework.Network
{
    public abstract class UUIDManager<TInstance, TTarget> : NetworkManagerBehaviour<TInstance>
        where TInstance : UUIDManager<TInstance, TTarget>
    {
        public event Action<TTarget> OnRegisterEvent;
        public event Action<TTarget> OnUnregisterEvent;

        protected override void Awake()
        {
            base.Awake();
            
            OnRegisterEvent = null;
            OnUnregisterEvent = null;
        }

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();
            
            UUIDCoreManager.Instance.OnUUIDOwnerRegistered += OnRegister;
            UUIDCoreManager.Instance.OnUUIDOwnerUnregistered += OnUnregister;
        }
        
        protected virtual void OnDestroy()
        {
            UUIDCoreManager.Instance.OnUUIDOwnerRegistered -= OnRegister;
            UUIDCoreManager.Instance.OnUUIDOwnerUnregistered -= OnUnregister;
        }

        #region Register & Unregister

        private void OnRegister(IUUIDOwner owner)
        {
            if (owner is not IController controller || controller.TryGetComponent(out TTarget target) == false)
            {
                return;
            }
            
            OnRegisterEvent?.Invoke(target);
        }
        
        private void OnUnregister(IUUIDOwner owner)
        {
            if (owner is not IController controller || controller.TryGetComponent(out TTarget target) == false)
            {
                return;
            }
            
            OnUnregisterEvent?.Invoke(target);
        }

        #endregion
    }
}

#endif