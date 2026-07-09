#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Localization;

namespace VMFramework.OdinExtensions
{
    internal sealed class LocaleNameAttributeDrawer : GeneralValueDropdownAttributeDrawer<LocaleNameAttribute>
    {
        protected override IEnumerable<ValueDropdownItem> GetValues()
        {
            return LocalizationNameListUtility.GetLocaleNameList();
        }
    }
}
#endif