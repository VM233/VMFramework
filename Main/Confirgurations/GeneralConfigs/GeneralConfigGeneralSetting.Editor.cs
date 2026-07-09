#if UNITY_EDITOR
using System.Linq;
using Sirenix.OdinInspector;
using VMFramework.Core.Editor;

namespace VMFramework.Configuration 
{
    public partial class GeneralConfigGeneralSetting
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();

            
        }

        [TabGroup(TAB_GROUP_NAME, GENERAL_CONFIGS_CATEGORY)]
        [Button]
        public void CollectGeneralConfigs()
        {
            generalConfigs.Clear();
            
            foreach (var generalConfig in typeof(GeneralConfig).FindAssetsOfType().Cast<GeneralConfig>())
            {
                generalConfigs.Add(generalConfig);
            }
            
            this.EnforceSave();
        }
    }
}
#endif