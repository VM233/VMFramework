using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.OdinExtensions;

namespace VMFramework.ResourcesManagement
{
    public class VisualEffectPropertiesAutoReset : MonoBehaviour
    {
        [TitleGroup(ComponentNames.CONFIG)]
        [VisualEffectPropertyName]
        [IsNotNullOrEmpty]
        public List<string> propertiesName = new();

        protected IPoolEventProvider poolEventProvider;
        protected VisualEffect visualEffect;

        protected readonly List<int> propertiesID = new();

        protected virtual void Awake()
        {
            poolEventProvider = GetComponentInParent<IPoolEventProvider>();
            poolEventProvider.OnReturnEvent += OnReturn;

            visualEffect = GetComponent<VisualEffect>();

            propertiesID.Clear();
            foreach (var propertyName in propertiesName)
            {
                propertiesID.Add(Shader.PropertyToID(propertyName));
            }
        }

        protected virtual void OnReturn(IReturnEventProvider provider)
        {
            foreach (var propertyID in propertiesID)
            {
                visualEffect.ResetOverride(propertyID);
            }
        }
    }
}