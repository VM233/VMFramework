#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.Core.Linq;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Editor.GameEditor
{
    public class GlobalSettingFileContextMenuProcessor
        : TypedFuncTargetsProcessor<IGlobalSettingFile, ContextMenuItemConfig>
    {
        protected override void ProcessTypedTargets(IReadOnlyList<IGlobalSettingFile> targets,
            ICollection<ContextMenuItemConfig> results)
        {
            results.Add(new(EditorNames.AUTO_FIND_SETTINGS_PATH,
                () => { targets.Examine(item => item.AutoFindSettings()); }));
            results.Add(new(EditorNames.AUTO_FIND_AND_CREATE_SETTINGS_PATH,
                () => { targets.Examine(item => item.AutoFindAndCreateSettings()); }));
            results.Add(new(EditorNames.OPEN_GLOBAL_SETTING_SCRIPT_PATH,
                () => { targets.Examine(item => item.OpenGlobalSettingScript()); }));
        }
    }
}
#endif