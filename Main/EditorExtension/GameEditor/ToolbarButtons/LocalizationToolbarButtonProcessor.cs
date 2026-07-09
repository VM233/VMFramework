#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.Localization;

namespace VMFramework.Editor.GameEditor
{
    public class LocalizationToolbarButtonProcessor
        : TypedFuncProcessor<ILocalizedStringOwnerConfig, ToolbarButtonConfig>
    {
        protected override void ProcessTypedTarget(ILocalizedStringOwnerConfig target,
            ICollection<ToolbarButtonConfig> results)
        {
            string tableName = null;
            if (target is ILocalizationTableNameOwner tableNameOwner)
            {
                tableName = tableNameOwner.LocalizationTableName;
            }

            results.Add(new(EditorNames.SET_DEFAULT_KEY_VALUE_PATH, () => target.SetDefaultKeyValue(new()
            {
                defaultTableName = tableName,
                save = true
            })));
            results.Add(new(EditorNames.REMOVE_INVALID_LOCALIZED_STRINGS_PATH, target.RemoveInvalidLocalizedStrings));
        }
    }
}
#endif