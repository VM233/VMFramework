﻿#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.UI
{
    public partial class UISettingFile : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => EditorNames.UI_SETTINGS;

        Icon IGameEditorMenuTreeNode.Icon => SdfIconType.Window;
    }
}
#endif