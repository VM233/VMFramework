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
        
        [UIPresetID(true)]
        [DisallowDuplicateElements]
        [ListDrawerSettings(ShowFoldout = false)]
        public List<string> uniqueUIPanelAutoOpenOnEnter = new();
        
        [UIPresetID]
        [DisallowDuplicateElements]
        [ListDrawerSettings(ShowFoldout = false)]
        public List<string> uiPanelAutoCloseOnEnter = new();
        
        [UIPresetID(true)]
        [DisallowDuplicateElements]
        [ListDrawerSettings(ShowFoldout = false)]
        public List<string> uniqueUIPanelAutoOpenOnExit = new();
        
        [UIPresetID]
        [DisallowDuplicateElements]
        [ListDrawerSettings(ShowFoldout = false)]
        public List<string> uiPanelAutoCloseOnExit = new();

        string IIDOwner<string>.id => procedureID;
    }
}