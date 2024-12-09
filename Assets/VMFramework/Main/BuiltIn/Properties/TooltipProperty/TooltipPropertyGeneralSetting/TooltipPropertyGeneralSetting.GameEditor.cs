#if UNITY_EDITOR
using VMFramework.Editor.GameEditor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Properties
{
    public partial class TooltipPropertyGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Property Tooltip";
    }
}
#endif