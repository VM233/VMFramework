﻿#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.ResourcesManagement
{
    public partial class ModelGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.name => "Model Prefab";

        Icon IGameEditorMenuTreeNode.Icon => SdfIconType.BoxSeam;
    }
}
#endif