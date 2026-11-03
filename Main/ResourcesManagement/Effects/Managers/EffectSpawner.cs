using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;
using VMFramework.Tools;

namespace VMFramework.Effects
{
    [ManagerCreationProvider(ManagerType.ResourcesCore)]
    public class EffectSpawner : ManagerBehaviour<IEffectSpawner>, IEffectSpawner
    {
        public Transform Container {get; private set;}

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            Container = ContainerTransform.Get(ResourcesManagementSetting.EffectGeneralSetting.containerName);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEffect Spawn(string id, Vector3 position, bool isWorldSpace, Transform parent = null,
            bool alignToParent = false)
        {
            var effect = GameItemManager.Instance.Get<IEffect>(id);
            
            effect.SetSpawnTransform(position, isWorldSpace, parent, alignToParent);

            return effect;
        }
#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button]
        private void SpawnFromInspector(
            [VMFramework.OdinExtensions.GamePrefabID(typeof(IEffectConfig))] string id,
            Vector3 position, bool isWorldSpace, Transform parent = null) =>
            Spawn(id, position, isWorldSpace, parent);
#endif
    }
}
