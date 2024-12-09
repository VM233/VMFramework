﻿#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public partial class TooltipGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Tooltip";

        Icon IGameEditorMenuTreeNode.Icon => SdfIconType.CardHeading;
    }
}
#endif