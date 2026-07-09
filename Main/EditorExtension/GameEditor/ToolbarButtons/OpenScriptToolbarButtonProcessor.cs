#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.Core.Editor;

namespace VMFramework.Editor.GameEditor
{
    public class OpenScriptToolbarButtonProcessor : IFuncProcessor<object, ToolbarButtonConfig>
    {
        public void ProcessTarget(object target, ICollection<ToolbarButtonConfig> results)
        {
            results.Add(new(EditorNames.OPEN_THIS_SCRIPT_PATH, target.OpenScriptOfObject));
            results.Add(new(EditorNames.SELECT_THIS_SCRIPT_PATH, target.SelectScriptOfObject));
        }
    }
}
#endif