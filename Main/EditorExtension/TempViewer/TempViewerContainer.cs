#if UNITY_EDITOR
using Sirenix.OdinInspector;

namespace VMFramework.Editor
{
    public sealed class TempViewerContainer : UnityEngine.ScriptableObject
    {
        [HideLabel]
        [System.NonSerialized, ShowInInspector]
        public object obj;
    }
}
#endif
