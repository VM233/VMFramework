#if UNITY_EDITOR
using VMFramework.Localization;

namespace VMFramework.GameLogicArchitecture
{
    public partial class LocalizedGameTagInfo : ILocalizedStringOwnerConfig
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();
                
            name ??= new();
        }
        
        public void AutoConfigureLocalizedString(LocalizedStringAutoConfigSettings settings)
        {
            name ??= new();

            name.AutoConfigNameByID(id, settings.defaultTableName);
        }

        public void SetKeyValueByDefault()
        {
            name.SetKeyValueByDefault();
        }
    }
}
#endif