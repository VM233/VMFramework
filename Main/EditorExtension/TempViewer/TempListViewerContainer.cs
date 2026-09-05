#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace VMFramework.Editor
{
    public sealed class TempListViewerContainer : UnityEngine.ScriptableObject
    {
        [System.NonSerialized, ShowInInspector]
        public List<object> objects;
    }
}
#endif
