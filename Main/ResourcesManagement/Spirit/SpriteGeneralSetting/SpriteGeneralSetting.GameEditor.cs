#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.ResourcesManagement
{
    public partial class SpriteGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Sprite Preset";

        EditorIcon IEditorIconProvider.Icon => SdfIconType.CardImage;
    }
}
#endif