#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using VMFramework.Configuration;
using VMFramework.Core.Editor;

namespace VMFramework.Editor.GameEditor
{
    public class OpenScriptContextMenuProcessor : IFuncTargetsProcessor<object, ContextMenuItemConfig>
    {
        public virtual void ProcessTargets(IReadOnlyCollection<object> targets,
            ICollection<ContextMenuItemConfig> results)
        {
            results.Add(new(EditorNames.OPEN_THIS_SCRIPT_PATH, targets.OpenScriptOfObjects));

            if (targets.Count == 1)
            {
                results.Add(new(EditorNames.SELECT_THIS_SCRIPT_PATH, targets.First().SelectScriptOfObject));
            }
        }
    }
}
#endif