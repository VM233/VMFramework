using System;

namespace VMFramework.UI
{
    internal sealed class UIPanelPointerWatch
    {
        private readonly IUIPanel panel;
        private readonly IUIPanelPointerEventProvider provider;
        private IUIPanel.OpenHandler open;
        private IUIPanel.CloseHandler close;
        private IUIPanelPointerEventProvider pointer;

        public UIPanelPointerWatch(IUIPanel panel, IUIPanelPointerEventProvider provider)
        { this.panel = panel; this.provider = provider; }

        public void Acquire(IUIPanel.OpenHandler onOpen, IUIPanel.CloseHandler onClose)
        {
            panel.OnOpen += onOpen; open = onOpen;
            panel.OnPostClose += onClose; close = onClose;
        }

        public void Activate(Action<IUIPanel> enter, Action<IUIPanel> leave)
        {
            pointer = provider;
            try { provider.AddPointerEvent(enter, leave); }
            catch (Exception primary)
            {
                var failures = new UIPanelReleaseFailures(nameof(UIPanelPointerWatch), primary);
                failures.Attempt("partial-pointer.release", ReleasePointer);
                failures.ThrowIfFailed(); throw;
            }
        }

        public void ReleasePointer()
        {
            var actual = pointer; pointer = null;
            actual?.RemovePointerEvent();
        }

        public void Release()
        {
            var opened = open; open = null;
            var closed = close; close = null;
            var failures = new UIPanelReleaseFailures(nameof(UIPanelPointerWatch));
            if (opened != null) failures.Attempt("panel-open.unsubscribe", () => panel.OnOpen -= opened);
            if (closed != null) failures.Attempt("panel-close.unsubscribe", () => panel.OnPostClose -= closed);
            failures.Attempt("pointer.release", ReleasePointer);
            failures.ThrowIfFailed();
        }
    }
}
