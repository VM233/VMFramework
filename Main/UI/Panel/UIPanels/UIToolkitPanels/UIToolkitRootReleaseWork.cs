using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace VMFramework.UI
{
    internal sealed class UIToolkitRootReleaseWork
    {
        public VisualElement Root;
        public CancellationTokenSource Cancellation;
        public CancellationToken Token;
        public Task Layout;
        public UIToolkitPointerEndpoints Pointer;
        public Exception SourceFailure;
        public Action AfterLayout;
        public Action<UIToolkitRootReleaseWork> Retirement;

        public UIToolkitRootReleaseWork(VisualElement root) => Root = root;

        public void CancelLayout()
        {
            var actual = Cancellation; Cancellation = null;
            if (actual == null) return;
            var failures = new UIPanelReleaseFailures(nameof(UIToolkitRootReleaseWork), SourceFailure);
            failures.Attempt("layout.cancel", actual.Cancel);
            failures.Attempt("layout-cts.dispose", actual.Dispose);
            failures.ThrowIfReleaseFailed();
        }

        public void ReleasePointer()
        {
            var actual = Pointer; Pointer = null;
            actual?.Release();
        }
    }
}
