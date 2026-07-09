#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMFramework.Procedure;

namespace VMFramework.Editor
{
    public class InitializerScriptCreationViewer : ScriptCreationViewer
    {
        [EnumToggleButtons]
        public InitializationOrder initializationOrder;
    }
}
#endif