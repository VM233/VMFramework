#if UNITY_EDITOR && FISHNET
using Sirenix.OdinInspector;
using VMFramework.Editor;
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Network 
{
    public partial class NetworkSettingFile : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Network Setting";

        Icon IGameEditorMenuTreeNode.Icon => SdfIconType.HddNetwork;
    }
}
#endif