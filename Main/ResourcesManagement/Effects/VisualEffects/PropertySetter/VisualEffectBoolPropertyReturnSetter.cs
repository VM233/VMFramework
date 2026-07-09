using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;
using VMFramework.Core;
using VMFramework.Effects;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.ResourcesManagement
{
    public class VisualEffectBoolPropertyReturnSetter : MonoBehaviour
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [VisualEffectPropertyName(typeof(bool))]
        [IsNotNullOrEmpty]
        public string propertyName;

        [TitleGroup(ComponentNames.CONFIG)]
        public bool value;

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
            visualEffect.SetBool(propertyName, value);
        }
    }
}