#if UNITY_EDITOR
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Editor
{
    public partial class TextureImporterGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Texture Importer";
    }
}
#endif