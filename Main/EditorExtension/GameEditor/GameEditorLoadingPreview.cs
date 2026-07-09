using Sirenix.OdinInspector;

namespace VMFramework.Editor.GameEditor
{
    internal sealed class GameEditorLoadingPreview
    {
        [ShowInInspector]
        [EnableGUI]
        [HideLabel]
        [DisplayAsString]
        private const string LOADING_PREVIEW_TEXT = "Loading Game Editor....";
    }
}