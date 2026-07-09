#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;
using VMFramework.Procedure;
using VMFramework.Procedure.Editor;

namespace VMFramework.Editor
{
    internal sealed class EditorInitializerViewerContainer : SimpleOdinEditorWindowContainer
    {
        [HideLabel]
        [EnableGUI]
        [ShowInInspector]
        public IReadOnlyInitializerManager initializerManager => EditorInitializer.InitializerManager;
    }
}
#endif