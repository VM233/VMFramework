#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.Core.Linq;
using VMFramework.Localization;

namespace VMFramework.Editor.GameEditor
{
    public class LocalizationContextMenuProcessor
        : TypedFuncTargetsProcessor<ILocalizedStringOwnerConfig, ContextMenuItemConfig>
    {
        protected override void ProcessTypedTargets(IReadOnlyList<ILocalizedStringOwnerConfig> targets,
            ICollection<ContextMenuItemConfig> results)
        {
            results.Add(new(EditorNames.SET_DEFAULT_KEY_VALUE_PATH, () =>
            {
                targets.Examine(config =>
                {
                    string tableName = null;
                    if (config is ILocalizationTableNameOwner tableNameOwner)
                    {
                        tableName = tableNameOwner.LocalizationTableName;
                    }

                    config.SetDefaultKeyValue(new()
                    {
                        defaultTableName = tableName,
                        save = true
                    });
                });
            }));

            results.Add(new(EditorNames.REMOVE_INVALID_LOCALIZED_STRINGS_PATH,
                () => { targets.Examine(config => config.RemoveInvalidLocalizedStrings()); }));
        }
    }
}
#endif