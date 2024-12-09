﻿#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Containers
{
    public partial class ContainerGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Container";

        Icon IGameEditorMenuTreeNode.Icon => SdfIconType.Archive;
    }
}
#endif