using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.OdinExtensions;

namespace VMFramework.UI
{
    public sealed partial class UIPanelProcedureConfig : BaseConfig, IIDOwner<string>
    {
        [ProcedureID]
        [IsNotNullOrEmpty]
        public string procedureID;
        
        [GamePrefabID(typeof(IUIPanelConfig))]
        [UIPanelConfigID(true)]
        [DisallowDuplicateElements]
        [ListDrawerSettings(ShowFoldout = false)]
        public List<string> uniqueUIPanelAutoOpenOnEnter = new();
        
        [GamePrefabID(typeof(IUIPanelConfig))]
        [DisallowDuplicateElements]
        [ListDrawerSettings(ShowFoldout = false)]
        public List<string> uiPanelAutoCloseOnEnter = new();
        
        [GamePrefabID(typeof(IUIPanelConfig))]
        [UIPanelConfigID(true)]
        [DisallowDuplicateElements]
        [ListDrawerSettings(ShowFoldout = false)]
        public List<string> uniqueUIPanelAutoOpenOnExit = new();
        
        [GamePrefabID(typeof(IUIPanelConfig))]
        [DisallowDuplicateElements]
        [ListDrawerSettings(ShowFoldout = false)]
        public List<string> uiPanelAutoCloseOnExit = new();

        string IIDOwner<string>.id => procedureID;
    }
}