#if UNITY_EDITOR
using Sirenix.OdinInspector;

namespace VMFramework.Editor
{
    public sealed class TempViewerContainer : SerializedScriptableObject
    {
        [HideLabel]
        public object obj;
    }
}
#endif