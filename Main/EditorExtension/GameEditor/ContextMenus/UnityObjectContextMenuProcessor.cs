#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.Configuration;
using VMFramework.Core.Editor;
using VMFramework.Core.Linq;
using Object = UnityEngine.Object;

namespace VMFramework.Editor.GameEditor
{
    public class UnityObjectContextMenuProcessor : TypedFuncTargetsProcessor<Object, ContextMenuItemConfig>
    {
        protected override void ProcessTypedTargets(IReadOnlyList<Object> targets,
            ICollection<ContextMenuItemConfig> results)
        {
            if (targets.Count == 1)
            {
                var item = targets[0];
                results.Add(new(EditorNames.SELECT_ASSET_PATH, item.SelectObject));

                if (item.IsAddressableAsset())
                {
                    results.Add(
                        new(EditorNames.SELECT_ADDRESSABLE_GROUP_PATH, item.SelectEntryGroupInAddressableWindow));
                }
            }
            else
            {
                results.Add(new(EditorNames.SELECT_ASSET_PATH, targets.SelectObjects));
            }

            results.Add(new(EditorNames.DELETE_ASSET_PATH, targets.DeleteAssetsWithDialog));
            results.Add(new(EditorNames.OPEN_IN_EXPLORER_PATH, targets.OpenInExplorer));
            results.Add(new(EditorNames.OPEN_ASSET_IN_NEW_INSPECTOR_PATH,
                () => targets.Examine(item => item.OpenInNewInspector())));
            results.Add(new(EditorNames.SAVE, targets.EnforceSave));
        }
    }
}
#endif