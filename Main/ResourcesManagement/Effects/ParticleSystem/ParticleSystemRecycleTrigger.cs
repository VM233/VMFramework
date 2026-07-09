using UnityEngine;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Effects
{
    public sealed class ParticleSystemRecycleTrigger : MonoBehaviour
    {
        private IEffect effect;

        private void Awake()
        {
            effect = GetComponent<IEffect>();
        }

        private void OnParticleSystemStopped()
        {
            GameItemManager.Instance.Return(effect);
        }
    }
}