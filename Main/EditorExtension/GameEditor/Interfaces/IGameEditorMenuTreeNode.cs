#if UNITY_EDITOR
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Editor.GameEditor
{
    public interface IGameEditorMenuTreeNode : INameOwner, IEditorIconProvider
    {
        EditorIcon IEditorIconProvider.Icon => EditorIcon.None;

        public object ParentNode => null;

        public object VisualNode => null;
        
        public bool IsVisible => true;
    }
}
#endif