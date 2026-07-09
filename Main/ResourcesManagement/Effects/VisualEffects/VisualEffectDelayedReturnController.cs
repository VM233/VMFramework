using UnityEngine;
using UnityEngine.VFX;
using VMFramework.Effects;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.ResourcesManagement
{
    public class VisualEffectDelayedReturnController : MonoBehaviour
    {
        protected VisualEffect visualEffect;
        protected IEffect effect;

        protected virtual void Awake()
        {
            visualEffect = GetComponentInParent<VisualEffect>();
            effect = GetComponentInParent<IEffect>();
            effect.OnDelayedPreReturn += OnDelayedPreReturn;
        }

        protected virtual void OnDelayedPreReturn(IGameItemDelayedReturnDispatcher dispatcher)
        {
            visualEffect.Stop();
        }
    }
}