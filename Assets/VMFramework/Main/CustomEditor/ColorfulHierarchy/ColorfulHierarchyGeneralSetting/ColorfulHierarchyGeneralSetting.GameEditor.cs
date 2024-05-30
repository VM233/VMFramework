﻿#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;
using VMFramework.Localization;

namespace VMFramework.Editor
{
    public partial class ColorfulHierarchyGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.name => "Colorful Hierarchy";

        string IGameEditorMenuTreeNode.folderPath => GameEditorNames.EDITOR_CATEGORY;

        Icon IGameEditorMenuTreeNode.icon => new(SdfIconType.FileEarmarkRichtextFill);
    }
}
#endif