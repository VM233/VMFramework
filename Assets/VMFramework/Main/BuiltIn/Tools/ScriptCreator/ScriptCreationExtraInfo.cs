#if UNITY_EDITOR
using System.Collections.Generic;

namespace VMFramework.Tools.Editor
{
    public class ScriptCreationExtraInfo
    {
        public string NamespaceName { get; init; }
        
        public IEnumerable<string> UsingNamespaces { get; init; }
    }
}
#endif