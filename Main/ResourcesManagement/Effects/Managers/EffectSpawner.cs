using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;
using VMFramework.Tools;

namespace VMFramework.Effects
{
    [ManagerCreationProvider(ManagerType.ResourcesCore)]
    public partial class EffectSpawner : ManagerBehaviour<IEffectSpawner>, IEffectSpawner
    {
        public Transform Container {get; private set;}

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            Container = ContainerTransform.Get(ResourcesManagementSetting.EffectGeneralSetting.containerName);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEffect Spawn(string id, Vector3 position, bool isWorldSpace, Transform parent = null)
        {
            var effect = GameItemManager.Instance.Get<IEffect>(id);
            
            if (parent != null)
            {
                effect.transform.SetParent(parent);
            }

            if (isWorldSpace)
            {
                effect.transform.position = position;
            }
            else
            {
                effect.transform.localPosition = position;
            }
            
            return effect;
        }
    }
}