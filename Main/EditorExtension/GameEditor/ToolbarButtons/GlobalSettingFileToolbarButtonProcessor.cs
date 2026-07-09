#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Editor.GameEditor
{
    public class GlobalSettingFileToolbarButtonProcessor : TypedFuncProcessor<IGlobalSettingFile, ToolbarButtonConfig>
    {
        protected override void ProcessTypedTarget(IGlobalSettingFile target, ICollection<ToolbarButtonConfig> results)
        {
            results.Add(new(EditorNames.AUTO_FIND_SETTINGS_PATH, target.AutoFindSettings));
            results.Add(new(EditorNames.AUTO_FIND_AND_CREATE_SETTINGS_PATH, target.AutoFindAndCreateSettings));
            results.Add(new(EditorNames.OPEN_GLOBAL_SETTING_SCRIPT_PATH, target.OpenGlobalSettingScript));
        }
    }
}
#endif