using System;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;
using Object = UnityEngine.Object;

namespace VMFramework.Effects
{
    public partial class EffectConfig : GamePrefab, IEffectConfig, IPrefabConfig
    {
        public override string IDSuffix => "effect";

        public override Type GameItemType => typeof(Effect);

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [Required]
        [ComponentRequired(nameof(GameItemType))]
        public GameObject prefab;

        #region Interface Implementation

        GameObject IPrefabProvider.Prefab => prefab;

        void IPrefabConfig.SetPrefab(GameObject prefab) => this.prefab = prefab;

        IGameItem IGamePrefab.GenerateGameItem()
        {
            return (IGameItem)Object.Instantiate(prefab).GetComponent(GameItemType);
        }

        #endregion
    }
}