using System;
using NUnit.Framework;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Tests
{
    public sealed class GameItemMaterializationTests
    {
        private sealed class RentalItem : ICreatablePoolItem<string>
        {
            public Action Initialize;
            public int Created;
            public int Rented;

            public void OnCreate(string argument)
            {
                Created++;
                Initialize();
            }

            public void OnGet()
            {
                Rented++;
                Initialize();
            }

            public void OnReturn() { }

            public void OnClear() { }
        }

        [Test]
        public void FreshAndPooledRentalsObserveTheirOwnInitializationKind()
        {
            var materialization = new GameItemMaterialization();
            var item = new RentalItem();
            var pool = new CreatablePoolItemsPool<RentalItem, string>("fixture", _ => item, 2);
            item.Initialize = () => Assert.That(materialization.CurrentKind,
                Is.EqualTo(GameItemInitializationKind.ClonedState));

            Assert.That(materialization.Rent(pool, GameItemInitializationKind.ClonedState), Is.SameAs(item));
            Assert.That(item.Created, Is.EqualTo(1));
            pool.Return(item);
            item.Initialize = () => Assert.That(materialization.CurrentKind,
                Is.EqualTo(GameItemInitializationKind.AuthoredDefaults));
            Assert.That(materialization.Rent(pool, GameItemInitializationKind.AuthoredDefaults), Is.SameAs(item));
            Assert.That(item.Created, Is.EqualTo(1));
            Assert.That(item.Rented, Is.EqualTo(1));
            pool.Return(item);
            Assert.Throws<InvalidOperationException>(() => _ = materialization.CurrentKind);
        }

        [Test]
        public void NestedRentalRestoresTheParentCloneContext()
        {
            var materialization = new GameItemMaterialization();
            var parent = new RentalItem();
            var child = new RentalItem();
            var childPool = new CreatablePoolItemsPool<RentalItem, string>("child", _ => child, 2);
            var parentPool = new CreatablePoolItemsPool<RentalItem, string>("parent", _ => parent, 2);
            child.Initialize = () => Assert.That(materialization.CurrentKind,
                Is.EqualTo(GameItemInitializationKind.AuthoredDefaults));
            parent.Initialize = () =>
            {
                Assert.That(materialization.CurrentKind, Is.EqualTo(GameItemInitializationKind.ClonedState));
                materialization.Rent(childPool, GameItemInitializationKind.AuthoredDefaults);
                Assert.That(materialization.CurrentKind, Is.EqualTo(GameItemInitializationKind.ClonedState));
                childPool.Return(child);
            };

            materialization.Rent(parentPool, GameItemInitializationKind.ClonedState);
            parentPool.Return(parent);
            Assert.Throws<InvalidOperationException>(() => _ = materialization.CurrentKind);
        }

        [Test]
        public void FailedNestedInitializationRetiresOnlyItsContext()
        {
            var materialization = new GameItemMaterialization();
            var parent = new RentalItem();
            var child = new RentalItem();
            var originalError = new InvalidOperationException("fixture initialization failed");
            var childPool = new CreatablePoolItemsPool<RentalItem, string>("child", _ => child, 2);
            var parentPool = new CreatablePoolItemsPool<RentalItem, string>("parent", _ => parent, 2);
            child.Initialize = () => throw originalError;
            parent.Initialize = () =>
            {
                Assert.That(Assert.Throws<InvalidOperationException>(() =>
                    materialization.Rent(childPool, GameItemInitializationKind.AuthoredDefaults)),
                    Is.SameAs(originalError));
                Assert.That(materialization.CurrentKind, Is.EqualTo(GameItemInitializationKind.ClonedState));
            };

            materialization.Rent(parentPool, GameItemInitializationKind.ClonedState);
            parentPool.Return(parent);
            Assert.Throws<InvalidOperationException>(() => _ = materialization.CurrentKind);
        }

        [Test]
        public void FailedRootInitializationPreservesTheOriginalErrorAndClearsContext()
        {
            var materialization = new GameItemMaterialization();
            var originalError = new InvalidOperationException("fixture initialization failed");
            var item = new RentalItem {Initialize = () => throw originalError};
            var pool = new CreatablePoolItemsPool<RentalItem, string>("fixture", _ => item, 2);

            Assert.That(Assert.Throws<InvalidOperationException>(() =>
                materialization.Rent(pool, GameItemInitializationKind.ClonedState)), Is.SameAs(originalError));
            Assert.Throws<InvalidOperationException>(() => _ = materialization.CurrentKind);
        }
    }
}
