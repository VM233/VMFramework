using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;
using VMFramework.Tools;

namespace VMFramework.ResourcesManagement
{
    [ManagerCreationProvider(ManagerType.ResourcesCore)]
    public sealed partial class EffectSpawner : ManagerBehaviour<EffectSpawner>
    {
        public static Transform Container {get; private set;}

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            Container = ContainerTransform.Get(ResourcesManagementSetting.EffectGeneralSetting.containerName);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEffect Spawn(string id, Vector3 position, bool isWorldSpace, Transform parent = null)
        {
            var effect = GameItemManager.Get<IEffect>(id);

            if (isWorldSpace)
            {
                effect.transform.position = position;
            }
            else
            {
                effect.transform.localPosition = position;
            }

            if (parent != null)
            {
                effect.transform.SetParent(parent);
            }
            
            return effect;
        }
    }
}