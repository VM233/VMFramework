using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Maps 
{
    public sealed partial class ShadowCaster2DGeneralSetting : GeneralSetting
    {
        private const string PREFAB_CATEGORY = "Prefabs";
        
        [TabGroup(TAB_GROUP_NAME, PREFAB_CATEGORY)]
        [Required]
        [RequiredComponent(typeof(BoxCollider2D))]
        [RequiredComponent(typeof(ShadowCaster2D))]
        public GameObject boxShaderCasterPrefab;
        
        [TabGroup(TAB_GROUP_NAME, PREFAB_CATEGORY)]
        [Required]
        [RequiredComponent(typeof(PolygonCollider2D))]
        [RequiredComponent(typeof(ShadowCaster2D))]
        public GameObject polygonShaderCasterPrefab;
        
        [TabGroup(TAB_GROUP_NAME, PREFAB_CATEGORY)]
        [Required]
        [RequiredComponent(typeof(CompositeCollider2D))]
        [RequiredComponent(typeof(ShadowCaster2D))]
        public GameObject compositeShaderCasterPrefab;
    }
}