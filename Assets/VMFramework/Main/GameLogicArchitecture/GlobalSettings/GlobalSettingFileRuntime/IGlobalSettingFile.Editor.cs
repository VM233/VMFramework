#if UNITY_EDITOR
using System;

namespace VMFramework.GameLogicArchitecture
{
    public partial interface IGlobalSettingFile
    {
        public void AutoFindSettings();

        public void AutoFindAndCreateSettings();
    }
}
#endif