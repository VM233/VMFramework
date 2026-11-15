using NUnit.Framework;
using UnityEngine.UIElements;
using VMFramework.UI;

namespace VMFramework.Editor.Tests
{
    public sealed class UIToolkitPresentationGateTests
    {
        [TestCase(false)]
        [TestCase(true)]
        public void LayoutAndEveryPlacementHoldPrecedeOpacityAndPickingAdmission(bool ignoreMouseEvents)
        {
            var root = new VisualElement { pickingMode = PickingMode.Ignore };
            root.style.opacity = .8f;
            var button = new Button();
            var passThrough = new VisualElement { pickingMode = PickingMode.Ignore };
            root.Add(button);
            root.Add(passThrough);
            var visibility = root.style.visibility;
            using var gate = new UIToolkitPresentationGate(root, ignoreMouseEvents);
            var first = gate.Hold();
            var second = gate.Hold();
            gate.ExcludePicking();
            Assert.That(root.style.opacity.value, Is.Zero);
            Assert.That(button.pickingMode, Is.EqualTo(PickingMode.Ignore));
            gate.CompleteLayout();
            first.Dispose();
            first.Dispose();
            Assert.That(root.style.opacity.value, Is.Zero, "A second owner still holds actual placement.");
            second.Dispose();
            Assert.That(root.style.opacity.value, Is.EqualTo(.8f));
            Assert.That(button.pickingMode, Is.EqualTo(ignoreMouseEvents ? PickingMode.Ignore : PickingMode.Position));
            Assert.That(passThrough.pickingMode, Is.EqualTo(PickingMode.Ignore));
            Assert.That(root.style.visibility, Is.EqualTo(visibility),
                "Admission must not dirty inherited visibility across the full tree.");
        }

        [Test]
        public void RetirementRestoresUnpresentedTreeAndLatePlacementCannotRepublishIt()
        {
            var root = new VisualElement();
            var button = new Button();
            root.Add(button);
            var opacity = root.style.opacity;
            var gate = new UIToolkitPresentationGate(root, ignoreMouseEvents: false);
            var placement = gate.Hold();
            gate.ExcludePicking();
            gate.Dispose();
            placement.Dispose();
            Assert.That(root.style.opacity, Is.EqualTo(opacity));
            Assert.That(button.pickingMode, Is.EqualTo(PickingMode.Position));
        }

        [Test]
        public void InitialBindingControlsJoinTheSameInputExclusionAndRestoreTheirAuthoredModes()
        {
            var root = new VisualElement();
            using var gate = new UIToolkitPresentationGate(root, ignoreMouseEvents: false);
            gate.ExcludePicking();
            var button = new Button();
            root.Add(button);
            gate.ExcludePicking();
            Assert.That(button.pickingMode, Is.EqualTo(PickingMode.Ignore));
            gate.CompleteLayout();
            Assert.That(button.pickingMode, Is.EqualTo(PickingMode.Position));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ContentPolicyReadsLogicalModesAndCanChangeThemWhileNativeInputIsMasked(bool ignoreMouseEvents)
        {
            var root = new VisualElement();
            var gate = new UIToolkitPresentationGate(root, ignoreMouseEvents);
            gate.ExcludePicking();
            var generated = new Button();
            root.Add(generated);
            gate.ExcludePicking();
            Assert.That(gate.GetContentPickingMode(generated), Is.EqualTo(PickingMode.Position));
            gate.SetContentPickingMode(generated, PickingMode.Ignore);
            Assert.That(gate.GetContentPickingMode(generated), Is.EqualTo(PickingMode.Ignore));
            gate.SetContentPickingMode(generated, PickingMode.Position);
            Assert.That(generated.pickingMode, Is.EqualTo(PickingMode.Ignore));
            gate.CompleteLayout();
            Assert.That(generated.pickingMode,
                Is.EqualTo(ignoreMouseEvents ? PickingMode.Ignore : PickingMode.Position));
            Assert.That(gate.GetContentPickingMode(generated), Is.EqualTo(PickingMode.Position));
            gate.Dispose();
            Assert.That(generated.pickingMode, Is.EqualTo(PickingMode.Position));
            gate.SetContentPickingMode(generated, PickingMode.Ignore);
            using var reopened = new UIToolkitPresentationGate(root, ignoreMouseEvents: false);
            reopened.ExcludePicking();
            reopened.CompleteLayout();
            Assert.That(generated.pickingMode, Is.EqualTo(PickingMode.Ignore),
                "A retired content owner may restore its original policy before another root publishes.");
        }

        [Test]
        public void NativeScrollViewPartsParticipateAlongsideLogicalContent()
        {
            var root = new VisualElement();
            var scroll = new ScrollView();
            root.Add(scroll);
            var nativePart = scroll.verticalScroller;
            Assert.That(scroll.contentContainer.Contains(nativePart), Is.False);
            nativePart.pickingMode = PickingMode.Position;
            using var gate = new UIToolkitPresentationGate(root, ignoreMouseEvents: false);
            gate.ExcludePicking();
            Assert.That(nativePart.pickingMode, Is.EqualTo(PickingMode.Ignore));
            Assert.That(gate.GetContentPickingMode(nativePart), Is.EqualTo(PickingMode.Position));
            gate.CompleteLayout();
            Assert.That(nativePart.pickingMode, Is.EqualTo(PickingMode.Position));
        }
    }
}
