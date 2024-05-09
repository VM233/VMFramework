﻿#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.GameLogicArchitecture;
using VMFramework.Localization;

namespace VMFramework.ResourcesManagement
{
    public partial class SpriteGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.name => new LocalizedTempString()
        {
            { "zh-CN", "精灵预设" },
            { "en-US", "Sprite Preset" }
        };

        string IGameEditorMenuTreeNode.folderPath => GameEditorNames.resourcesManagementCategoryName;

        EditorIconType IGameEditorMenuTreeNode.iconType => EditorIconType.SdfIcon;

        SdfIconType IGameEditorMenuTreeNode.sdfIcon => SdfIconType.CardImage;
    }
}
#endif