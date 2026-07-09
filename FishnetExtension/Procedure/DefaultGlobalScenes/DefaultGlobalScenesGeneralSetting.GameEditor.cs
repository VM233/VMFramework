#if UNITY_EDITOR
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Procedure 
{
    public partial class DefaultGlobalScenesGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Default Global Scenes";
    }
}
#endif