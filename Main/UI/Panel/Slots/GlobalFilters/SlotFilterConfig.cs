using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public class SlotFilterConfig : GamePrefab
    {
        public override string IDSuffix => "slot_filter";

        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        public StyleSheet nonFilteredStyleSheet;
        
        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        public StyleSheet matchedStyleSheet;
        
        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        public StyleSheet unmatchedStyleSheet;
    }
}