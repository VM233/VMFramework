#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using EditorIcon = VMFramework.Editor.EditorIcon;

namespace VMFramework.GameLogicArchitecture.Editor
{
    public partial class EditorSettingFile : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => EditorNames.EDITOR_SETTINGS;

        public EditorIcon Icon => EditorIcons.UnityLogo;
    }
}
#endif