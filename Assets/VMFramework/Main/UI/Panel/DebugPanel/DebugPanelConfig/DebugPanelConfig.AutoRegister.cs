using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;
using VMFramework.Procedure;

namespace VMFramework.UI
{
    [GamePrefabTypeAutoRegister(ID)]
    public partial class DebugPanelConfig : IGamePrefabAutoRegisterProvider
    {
        public const string ID = "debug_screen_ui";
        
        public const string LEFT_GROUP_NAME = "LeftGroup";

        public const string RIGHT_GROUP_NAME = "RightGroup";
        
        void IGamePrefabAutoRegisterProvider.OnGamePrefabAutoRegister()
        {
            leftContainerVisualElementName = LEFT_GROUP_NAME;
            rightContainerVisualElementName = RIGHT_GROUP_NAME;
            
            var procedureConfigs = UISetting.UIPanelProcedureGeneralSetting.procedureConfigs;

            procedureConfigs.TryAddConfig(new UIPanelProcedureConfig()
            {
                procedureID = MainMenuProcedure.ID
            });

            if (procedureConfigs.TryGetConfig(MainMenuProcedure.ID, out var mainMenuConfig))
            {
                mainMenuConfig.uniqueUIPanelAutoOpenOnEnter.AddIfNotContains(id);
            }
        }
    }
}