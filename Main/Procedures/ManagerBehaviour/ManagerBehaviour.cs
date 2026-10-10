using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMFramework.Procedure
{
    /// <summary>
    /// 非线程安全的管理器基类，用于实现单例。
    /// Non-thread-safe manager base class used for singleton implementation.
    /// </summary>
    public class ManagerBehaviour<TInstance> : MonoBehaviour, IManagerBehaviour
        where TInstance : class
    {
        [ShowInInspector]
        [ReadOnly]
        [HideInEditorMode]
        public static TInstance Instance { get; private set; }

        protected virtual void Awake()
        {
            if (this is not TInstance owner)
            {
                throw new InvalidOperationException($"Manager singleton publication rejected at Awake: " +
                    $"candidate '{GetType().FullName}' does not implement '{typeof(TInstance).FullName}'.");
            }

            if (!ReferenceEquals(Instance, null) && !ReferenceEquals(Instance, this))
            {
                throw new InvalidOperationException($"Manager singleton publication rejected at Awake: " +
                    $"owner '{Instance.GetType().FullName}' already owns '{typeof(TInstance).FullName}'; " +
                    $"candidate '{GetType().FullName}' is a different physical owner.");
            }

            Instance = owner;
        }

        protected virtual void OnDestroy()
        {
            if (ReferenceEquals(Instance, this)) Instance = null;
        }

        protected virtual void OnBeforeInitStart()
        {

        }

        protected virtual void GetInitializationActions(ICollection<InitializationAction> actions)
        {
            actions.Add(new(InitializationOrder.BeforeInitStart, OnBeforeInitStartInternal, this));
        }

        void IInitializer.GetInitializationActions(ICollection<InitializationAction> actions)
        {
            GetInitializationActions(actions);
        }

        private UniTask OnBeforeInitStartInternal(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            OnBeforeInitStart();
            return UniTask.CompletedTask;
        }
    }
}
