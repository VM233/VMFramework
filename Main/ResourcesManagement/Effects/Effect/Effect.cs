using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Effects
{
    public partial class Effect : ControllerGameItem, IEffect
    {
        [TitleGroup(ComponentNames.CONFIG)]
        public bool enableLogging;
        
        public event IGameItemDelayedReturnDispatcher.DelayedPreReturnHandler OnDelayedPreReturn;

        protected override void OnReturn()
        {
            base.OnReturn();

            transform.SetParent(EffectSpawner.Instance.Container);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public virtual void DelayedReturn()
        {
            OnDelayedPreReturn?.Invoke(this);
        }

        protected virtual void OnEnable()
        {
            if (enableLogging)
            {
                UnityEngine.Debug.LogError($"Entity {name} enabled", this);
            }
        }
        
        protected virtual void OnDisable()
        {
            if (enableLogging)
            {
                UnityEngine.Debug.LogError($"Entity {name} disabled", this);
            }
        }
    }
}