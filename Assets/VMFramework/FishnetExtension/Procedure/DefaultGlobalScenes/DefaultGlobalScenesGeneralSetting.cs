using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.GameLogicArchitecture;
using VMFramework.OdinExtensions;

namespace VMFramework.Procedure 
{
    public sealed partial class DefaultGlobalScenesGeneralSetting : GeneralSetting
    {
        public bool enableDefaultGlobalScenesLoader = true;
        
        [BuildSceneName]
        [IsNotNullOrEmpty]
        [EnableIf(nameof(enableDefaultGlobalScenesLoader))]
        public List<string> sceneNames = new();
    }
}