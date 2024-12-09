#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Cameras
{
    public partial class CameraGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Camera";

        Icon IGameEditorMenuTreeNode.Icon => SdfIconType.Camera;
    }
}
#endif