#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using VMFramework.Configuration;
using VMFramework.Core.Editor;

namespace VMFramework.Editor.GameEditor
{
    public class UnityObjectToolbarButtonProcessor : TypedFuncProcessor<Object, ToolbarButtonConfig>
    {
        protected override void ProcessTypedTarget(Object target, ICollection<ToolbarButtonConfig> results)
        {
            results.Add(new(EditorNames.SELECT_ASSET_PATH, target.SelectObject));
            results.Add(new(EditorNames.DELETE_ASSET_PATH, target.DeleteAssetWithDialog));
            results.Add(new(EditorNames.OPEN_IN_EXPLORER_PATH, target.OpenInExplorer));
            results.Add(new(EditorNames.OPEN_ASSET_IN_NEW_INSPECTOR_PATH, target.OpenInNewInspector));
            results.Add(new(EditorNames.SAVE, target.EnforceSave));

            if (target.IsAddressableAsset())
            {
                results.Add(new(EditorNames.SELECT_ADDRESSABLE_GROUP_PATH, target.SelectEntryGroupInAddressableWindow));
            }
        }
    }
}
#endif