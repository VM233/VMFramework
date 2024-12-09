using System;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;
using Object = UnityEngine.Object;

namespace VMFramework.ResourcesManagement 
{
    public partial class EffectConfig : GamePrefab, IEffectConfig
    {
        protected const string EFFECT_CATEGORY = "Effect";
        
        public override string IDSuffix => "effect";

        public override Type GameItemType => typeof(Effect);

        [TabGroup(TAB_GROUP_NAME, EFFECT_CATEGORY)]
        [Required]
        [RequiredComponent(nameof(GameItemType))]
        public GameObject prefab;

        GameObject IPrefabProvider.Prefab => prefab;

        IGameItem IGamePrefab.GenerateGameItem()
        {
            return (IGameItem)Object.Instantiate(prefab).GetComponent(GameItemType);
        }
    }
}