using System;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace VMFramework.UI
{
    public sealed class UIToolkitRootRetirement
    {
        public VisualElement Root { get; }
        public Exception Failure { get; }
        public Task LayoutCompletion { get; }
        internal UIToolkitRootRetirement(VisualElement root, Exception failure, Task layoutCompletion)
        { Root = root; Failure = failure; LayoutCompletion = layoutCompletion; }
    }
}
