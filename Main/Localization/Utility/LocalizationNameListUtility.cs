#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.Localization;

namespace VMFramework.Localization
{
    public static class LocalizationNameListUtility
    {
        #region Name List

        public static IEnumerable<ValueDropdownItem> GetTableNameList()
        {
            foreach (var collection in LocalizationEditorSettings.GetStringTableCollections())
            {
                yield return new ValueDropdownItem(collection.TableCollectionName, collection.TableCollectionName);
            }
        }

        public static IEnumerable<ValueDropdownItem> GetLocaleNameList()
        {
            foreach (var locale in LocalizationEditorSettings.GetLocales())
            {
                yield return new ValueDropdownItem(locale.Identifier.Code, locale.Identifier.Code);
            }
        }

        #endregion
    }
}
#endif