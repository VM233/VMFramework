using System.Linq;
using NUnit.Framework;
using UnityEngine.UIElements;
using VMFramework.UI;

namespace VMFramework.Tests
{
    public class SlotVisualElementTests
    {
        [Test]
        public void ConstructorPreservesSlotVisualHierarchy()
        {
            var slot = new SlotVisualElement();
            var inputElement = slot.Q<VisualElement>(className: "slot-input");

            Assert.That(inputElement, Is.SameAs(slot.hierarchy[0]));
            Assert.That(inputElement.childCount, Is.Zero);
            Assert.That(inputElement.pickingMode, Is.EqualTo(PickingMode.Ignore));
            Assert.That(slot.hierarchy.Children().Skip(1).Select(element => element.name), Is.EqualTo(new[]
            {
                "deeper-background",
                "background",
                "fore-background",
                "icon",
                "border",
                "description"
            }));
            Assert.That(slot.ClassListContains(BaseField<bool>.ussClassName), Is.False);
            Assert.That(slot.ClassListContains(BaseField<bool>.noLabelVariantUssClassName), Is.False);
        }

        [Test]
        public void ValueUsesNativeBooleanFieldContract()
        {
            var slot = new SlotVisualElement();

            Assert.That(slot, Is.InstanceOf<BaseBoolField>());
            Assert.That(slot.value, Is.False);

            slot.SetValueWithoutNotify(true);

            Assert.That(slot.value, Is.True);
        }
    }
}
