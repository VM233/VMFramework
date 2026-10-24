using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine.TestTools;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Tests
{
    public sealed class GlobalSettingLoaderPreparationTests
    {
        private static readonly FieldInfo Catalog = typeof(GlobalSettingFileManager)
            .GetField("loaders", BindingFlags.Static | BindingFlags.NonPublic);
        private object previousCatalog;
        private Dictionary<Type, IGlobalSettingFileLoader> initialCatalog;
        private static Action constructing;
        private static int constructed;

        [SetUp]
        public void SetUp()
        {
            previousCatalog = Catalog.GetValue(null);
            initialCatalog = new Dictionary<Type, IGlobalSettingFileLoader>();
            Catalog.SetValue(null, initialCatalog);
            constructing = null; constructed = 0;
        }

        [TearDown]
        public void TearDown()
        {
            Catalog.SetValue(null, previousCatalog);
            constructing = null; constructed = 0;
        }

        [UnityTest]
        public IEnumerator CompletedPreparationPublishesActualInstancesAndReusesThem()
            => UniTask.ToCoroutine(async () =>
            {
                await GlobalSettingFileManager.PrepareLoadersAsync(CancellationToken.None);
                var first = Current;
                Assert.That(first, Is.Not.SameAs(initialCatalog));
                Assert.That(first.ContainsKey(typeof(GlobalSettingFileAddressableLoader)), Is.True);
                Assert.That(first.ContainsKey(typeof(ObservedLoader)), Is.True);
                Assert.That(constructed, Is.EqualTo(1));
                await GlobalSettingFileManager.PrepareLoadersAsync(CancellationToken.None);
                Assert.That(Current, Is.Not.SameAs(first));
                Assert.That(Current[typeof(ObservedLoader)], Is.SameAs(first[typeof(ObservedLoader)]));
                Assert.That(Current[typeof(GlobalSettingFileAddressableLoader)], Is.SameAs(first[typeof(GlobalSettingFileAddressableLoader)]));
                Assert.That(constructed, Is.EqualTo(1));
            });

        [UnityTest]
        public IEnumerator AlreadyCanceledSourceDoesNotPublishACatalog()
            => UniTask.ToCoroutine(async () =>
            {
                using var cancellation = new CancellationTokenSource();
                cancellation.Cancel();
                await AssertCanceled(cancellation);
                Assert.That(Current, Is.SameAs(initialCatalog));
                Assert.That(constructed, Is.Zero);
            });

        [UnityTest]
        public IEnumerator CancellationBeforePublicationRetainsThePriorCompleteCatalog()
            => UniTask.ToCoroutine(async () =>
            {
                using var cancellation = new CancellationTokenSource();
                constructing = cancellation.Cancel;
                await AssertCanceled(cancellation);
                Assert.That(constructed, Is.EqualTo(1));
                Assert.That(Current, Is.SameAs(initialCatalog));
                Assert.That(Current, Is.Empty);
            });

        [UnityTest]
        public IEnumerator ConstructorFailureRetainsTheOriginalFailureAndCatalog()
            => UniTask.ToCoroutine(async () =>
            {
                var expected = new InvalidOperationException("Owned loader-construction failure.");
                constructing = () => throw expected;
                try
                {
                    await GlobalSettingFileManager.PrepareLoadersAsync(CancellationToken.None);
                    Assert.Fail("A failing constructor must not publish successful preparation.");
                }
                catch (TargetInvocationException failure)
                {
                    Assert.That(failure.InnerException, Is.SameAs(expected));
                }
                Assert.That(Current, Is.SameAs(initialCatalog));
                Assert.That(Current, Is.Empty);
            });

        private static async UniTask AssertCanceled(CancellationTokenSource cancellation)
        {
            try
            {
                await GlobalSettingFileManager.PrepareLoadersAsync(cancellation.Token);
                Assert.Fail("Canceled preparation must retain its actual token.");
            }
            catch (OperationCanceledException failure)
            {
                Assert.That(failure.CancellationToken, Is.EqualTo(cancellation.Token));
            }
        }

        private static Dictionary<Type, IGlobalSettingFileLoader> Current
            => (Dictionary<Type, IGlobalSettingFileLoader>)Catalog.GetValue(null);

        private sealed class ObservedLoader : IGlobalSettingFileLoader
        {
            public ObservedLoader() { constructed++; constructing?.Invoke(); }
            public GlobalSettingFileLoaderTargetConfig CanLoad(Type globalSettingType) => new(false, 0);
            public UniTask<IGlobalSettingFile> LoadGlobalSettingFile(Type globalSettingType, GlobalSettingFileConfigAttribute config)
                => throw new InvalidOperationException("Preparation fixtures do not load assets.");
        }
    }
}
