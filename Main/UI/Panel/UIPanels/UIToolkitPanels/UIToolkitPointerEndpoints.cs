using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace VMFramework.UI
{
    internal sealed class UIToolkitPointerEndpoints
    {
        private readonly EventCallback<MouseEnterEvent> enter;
        private readonly EventCallback<MouseLeaveEvent> leave;
        private readonly List<VisualElement> enterSources = new();
        private readonly List<VisualElement> leaveSources = new();

        public UIToolkitPointerEndpoints(EventCallback<MouseEnterEvent> enter,
            EventCallback<MouseLeaveEvent> leave)
        { this.enter = enter; this.leave = leave; }

        public void Acquire(VisualElement root)
        {
            foreach (var child in root.Children())
            {
                child.RegisterCallback(enter);
                try { enterSources.Add(child); }
                catch (Exception primary)
                {
                    var failures = new UIPanelReleaseFailures(nameof(UIToolkitPointerEndpoints), primary);
                    failures.Attempt("unadopted-enter.unsubscribe", () => child.UnregisterCallback(enter));
                    failures.ThrowIfFailed(); throw;
                }
                child.RegisterCallback(leave);
                try { leaveSources.Add(child); }
                catch (Exception primary)
                {
                    var failures = new UIPanelReleaseFailures(nameof(UIToolkitPointerEndpoints), primary);
                    failures.Attempt("unadopted-leave.unsubscribe", () => child.UnregisterCallback(leave));
                    failures.ThrowIfFailed(); throw;
                }
            }
        }

        public void Release()
        {
            var enters = enterSources.ToArray();
            var leaves = leaveSources.ToArray();
            enterSources.Clear(); leaveSources.Clear();
            var failures = new UIPanelReleaseFailures(nameof(UIToolkitPointerEndpoints));
            foreach (var source in enters)
                failures.Attempt("pointer-enter.unsubscribe", () => source.UnregisterCallback(enter));
            foreach (var source in leaves)
                failures.Attempt("pointer-leave.unsubscribe", () => source.UnregisterCallback(leave));
            failures.ThrowIfFailed();
        }
    }
}
