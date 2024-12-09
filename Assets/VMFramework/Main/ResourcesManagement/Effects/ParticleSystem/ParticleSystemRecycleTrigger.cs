using System;
using UnityEngine;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.ResourcesManagement
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
            GameItemManager.Return(effect);
        }
    }
}