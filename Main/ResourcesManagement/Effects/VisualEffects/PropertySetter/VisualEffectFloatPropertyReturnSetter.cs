using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;
using VMFramework.Core;
using VMFramework.Effects;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.ResourcesManagement
{
    public class VisualEffectFloatPropertyReturnSetter : MonoBehaviour
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [VisualEffectPropertyName(typeof(float))]
        [IsNotNullOrEmpty]
        public string propertyName;

        [TitleGroup(ComponentNames.CONFIG)]
        public float value;
        
        protected IEffect effect;
        protected VisualEffect visualEffect;

        protected virtual void Awake()
        {
            effect = GetComponentInParent<IEffect>();
            effect.OnDelayedPreReturn += OnDelayedPreReturn;
            
            visualEffect = GetComponentInChildren<VisualEffect>();
        }

        protected virtual void OnDelayedPreReturn(IGameItemDelayedReturnDispatcher dispatcher)
        {
            visualEffect.SetFloat(propertyName, value);
        }
    }
}