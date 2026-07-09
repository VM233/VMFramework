#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.OdinExtensions
{
    internal sealed class GamePrefabGeneralSettingAttributeDrawer : 
        GeneralValueDropdownAttributeDrawer<GamePrefabGeneralSettingAttribute>
    {
        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            return GamePrefabGeneralSettingUtility.GetGamePrefabGeneralSettingNameList();
        }
    }
}
#endif