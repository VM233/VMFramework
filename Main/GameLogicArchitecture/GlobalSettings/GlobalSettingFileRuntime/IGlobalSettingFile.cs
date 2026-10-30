using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    [UnityEngine.Scripting.RequireImplementors]
    public partial interface IGlobalSettingFile : IInitializer, ICheckableConfig
    {
        public IEnumerable<IGeneralSetting> GetAllGeneralSettings();
    }
}
