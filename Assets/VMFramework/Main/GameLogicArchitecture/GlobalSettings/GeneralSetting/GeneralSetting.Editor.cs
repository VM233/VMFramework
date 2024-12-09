#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector;

namespace VMFramework.GameLogicArchitecture
{
    public partial class GeneralSetting
    {
        protected override void OnInspectorInit()
        {
            base.OnInspectorInit();
            
            AutoConfigureLocalizedString(new()
            {
                defaultTableName = DefaultLocalizationTableName,
                save = false
            });
        }
    }
}
#endif