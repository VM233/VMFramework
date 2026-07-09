#if UNITY_EDITOR
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Effects
{
    public partial class EffectGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Effect";

        EditorIcon IEditorIconProvider.Icon => EditorIcon.None;
    }
}
#endif