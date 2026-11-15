using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace VMFramework.UI
{
    /// <summary>Owns pre-presentation rendering and input through native layout and placement leases.</summary>
    internal sealed class UIToolkitPresentationGate : IDisposable
    {
        private readonly VisualElement root;
        private readonly StyleFloat authoredOpacity;
        private readonly bool ignoreMouseEvents;
        private readonly Dictionary<VisualElement, PickingMode> picking = new();
        private bool layoutComplete;
        private bool retired;
        private bool admitted;
        private int holds;

        internal UIToolkitPresentationGate(VisualElement root, bool ignoreMouseEvents)
        {
            this.root = root;
            authoredOpacity = root.style.opacity;
            this.ignoreMouseEvents = ignoreMouseEvents;
            root.style.opacity = 0;
            root.RegisterCallback<PointerDownEvent>(BlockInput, TrickleDown.TrickleDown);
            root.RegisterCallback<PointerUpEvent>(BlockInput, TrickleDown.TrickleDown);
            root.RegisterCallback<ClickEvent>(BlockInput, TrickleDown.TrickleDown);
            root.RegisterCallback<WheelEvent>(BlockInput, TrickleDown.TrickleDown);
            root.RegisterCallback<KeyDownEvent>(BlockInput, TrickleDown.TrickleDown);
            root.RegisterCallback<NavigationSubmitEvent>(BlockInput, TrickleDown.TrickleDown);
        }

        private static void BlockInput<TEvent>(TEvent evt) where TEvent : EventBase<TEvent>, new() =>
            evt.StopImmediatePropagation();

        internal IDisposable Hold()
        {
            if (admitted || retired)
                throw new InvalidOperationException("Presentation holds must be acquired during root preparation.");
            holds++;
            return new PlacementHold(this);
        }

        internal void ExcludePicking()
        {
            Exclude(root);
            root.Query<VisualElement>().ForEach(Exclude);
        }

        private void Exclude(VisualElement element)
        {
            if (!picking.ContainsKey(element)) picking.Add(element, element.pickingMode);
            element.pickingMode = PickingMode.Ignore;
        }

        internal void CompleteLayout()
        {
            ExcludePicking();
            layoutComplete = true;
            TryAdmit();
        }

        private void ReleaseHold()
        {
            holds--;
            TryAdmit();
        }

        private void TryAdmit()
        {
            if (retired || !layoutComplete || holds != 0) return;
            admitted = true;
            RestoreInput(admitting: true);
            root.style.opacity = authoredOpacity;
        }

        private void RestoreInput(bool admitting = false)
        {
            foreach (var pair in picking)
                pair.Key.pickingMode = admitting && ignoreMouseEvents ? PickingMode.Ignore : pair.Value;
            picking.Clear();
            root.UnregisterCallback<PointerDownEvent>(BlockInput, TrickleDown.TrickleDown);
            root.UnregisterCallback<PointerUpEvent>(BlockInput, TrickleDown.TrickleDown);
            root.UnregisterCallback<ClickEvent>(BlockInput, TrickleDown.TrickleDown);
            root.UnregisterCallback<WheelEvent>(BlockInput, TrickleDown.TrickleDown);
            root.UnregisterCallback<KeyDownEvent>(BlockInput, TrickleDown.TrickleDown);
            root.UnregisterCallback<NavigationSubmitEvent>(BlockInput, TrickleDown.TrickleDown);
        }

        public void Dispose()
        {
            retired = true;
            RestoreInput();
            root.style.opacity = authoredOpacity;
        }

        private sealed class PlacementHold : IDisposable
        {
            private UIToolkitPresentationGate owner;
            internal PlacementHold(UIToolkitPresentationGate owner) => this.owner = owner;
            public void Dispose()
            {
                var actual = owner;
                owner = null;
                actual?.ReleaseHold();
            }
        }
    }
}
