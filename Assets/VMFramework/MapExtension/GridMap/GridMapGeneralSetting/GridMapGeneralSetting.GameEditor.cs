﻿#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Maps
{
    public partial class GridMapGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Grid Map";
        
        Icon IGameEditorMenuTreeNode.Icon => SdfIconType.PinMap;
    }
}
#endif