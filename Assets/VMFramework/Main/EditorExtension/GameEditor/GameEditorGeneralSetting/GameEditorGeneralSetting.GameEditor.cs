#if UNITY_EDITOR
using System.Collections.Generic;
using VMFramework.Core.Editor;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Editor.GameEditor
{
    public partial class GameEditorGeneralSetting : IGameEditorMenuTreeNode
    {
        string INameOwner.Name => "Game Editor";

        protected override IEnumerable<ToolbarButtonConfig> GetToolbarButtons()
        {
            yield return new(GameEditorNames.OPEN_GAME_EDITOR_SCRIPT_PATH, () =>
            {
                typeof(GameEditor).OpenScriptOfType();
            });
            
            foreach (var config in base.GetToolbarButtons())
            {
                yield return config;
            }
        }
    }
}
#endif