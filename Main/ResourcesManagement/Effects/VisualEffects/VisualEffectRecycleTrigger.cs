using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.Effects;
using VMFramework.GameLogicArchitecture;
using VMFramework.Timers;

namespace VMFramework.ResourcesManagement
{
    public class VisualEffectRecycleTrigger : MonoBehaviour, ITimer<double>
    {
        [MinValue(0)]
        public float checkInterval = 1f;
        
        protected VisualEffect visualEffect;
        protected IEffect effect;
        
        protected virtual void Awake()
        {
            visualEffect = GetComponentInParent<VisualEffect>();
            effect = GetComponentInParent<IEffect>();
            effect.OnGetEvent += OnGet;
        }

        protected virtual void OnGet(IPoolEventProvider provider)
        {
            TimerManager.Instance.Add(this, checkInterval);
        }

        protected virtual void OnTimed()
        {
            if (visualEffect.aliveParticleCount > 0)
            {
                TimerManager.Instance.Add(this, checkInterval);
            }
            else
            {
                GameItemManager.Instance.Return(effect);
            }
        }

        void ITimer<double>.OnTimed()
        {
            OnTimed();
        }

        #region Priority Queue Node

        double IGenericPriorityQueueNode<double>.Priority { get; set; }

        int IGenericPriorityQueueNode<double>.QueueIndex { get; set; }

        long IGenericPriorityQueueNode<double>.InsertionIndex { get; set; }

        #endregion
    }
}