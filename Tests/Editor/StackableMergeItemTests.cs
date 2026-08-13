using NUnit.Framework;
using VMFramework.Containers;

namespace VMFramework.Tests
{
    public class StackableMergeItemTests
    {
        [TestCase(5, 5, 5)]
        [TestCase(5, 2, 2)]
        [TestCase(5, 8, 5)]
        [TestCase(5, 0, 0)]
        public void RemovableCountMatchesStackRemovalCapacity(int itemCount, int targetRemoveCount,
            int expectedRemovableCount)
        {
            Assert.That(
                StackableMergeItem.CalculateRemovableCount(false, itemCount, targetRemoveCount),
                Is.EqualTo(expectedRemovableCount));
        }

        [Test]
        public void DestroyedItemHasNoRemovalCapacity()
        {
            Assert.That(StackableMergeItem.CalculateRemovableCount(true, 5, 5), Is.Zero);
        }
    }
}
