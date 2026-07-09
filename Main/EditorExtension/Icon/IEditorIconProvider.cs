#if UNITY_EDITOR
namespace VMFramework.Editor
{
    public interface IEditorIconProvider
    {
        public EditorIcon Icon { get; }
    }
}
#endif