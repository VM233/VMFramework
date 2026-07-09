#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace VMFramework.Editor
{
    public sealed class TempListViewerContainer : SerializedScriptableObject
    {
        public List<object> objects;
    }
}
#endif