using NUnit.Framework;
using UnityEngine.UIElements;
using VMFramework.UI;

namespace VMFramework.Tests
{
    public class BoolStateVisualElementTests
    {
        [Test]
        public void ConstructorExposesOnlyTheStateVisual()
        {
            var element = new BoolStateVisualElement();

            Assert.That(element.hierarchy.childCount, Is.Zero);
            Assert.That(element.pickingMode, Is.EqualTo(PickingMode.Ignore));
            Assert.That(element.focusable, Is.False);
            Assert.That(element.ClassListContains(BaseField<bool>.ussClassName), Is.False);
            Assert.That(element.ClassListContains(BaseField<bool>.noLabelVariantUssClassName), Is.False);
        }

        [Test]
        public void ValueUsesNativeBooleanFieldContract()
        {
            var element = new BoolStateVisualElement();

            Assert.That(element, Is.InstanceOf<BaseBoolField>());
            Assert.That(element.value, Is.False);

            element.SetValueWithoutNotify(true);

            Assert.That(element.value, Is.True);
        }
    }
}
