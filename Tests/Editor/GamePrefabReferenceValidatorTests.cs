using System.Linq;
using NUnit.Framework;
using UnityEngine;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Tests
{
    public sealed class GamePrefabReferenceValidatorTests
    {
        [System.Serializable]
        private sealed class TestGamePrefab : GamePrefab, IPrefabProvider
        {
            private readonly GameObject prefab;

            public TestGamePrefab(string id, GameObject prefab)
            {
                this.id = id;
                this.prefab = prefab;
            }

            GameObject IPrefabProvider.Prefab => prefab;
        }

        [Test]
        public void ValidPrefabReferencesPass()
        {
            var prefab = new GameObject("Valid Prefab");
            try
            {
                Assert.DoesNotThrow(() => GamePrefabReferenceValidator.Validate(
                    new IGamePrefab[]
                    {
                        new TestGamePrefab("valid_item", prefab),
                    }));
            }
            finally
            {
                Object.DestroyImmediate(prefab);
            }
        }

        [Test]
        public void AllMissingPrefabReferencesAreReportedInStableOrder()
        {
            var validPrefab = new GameObject("Valid Prefab");
            try
            {
                var exception = Assert.Throws<MissingGamePrefabReferencesException>(
                    () => GamePrefabReferenceValidator.Validate(
                        new IGamePrefab[]
                        {
                            new TestGamePrefab("zeta_item", null),
                            new TestGamePrefab("valid_item", validPrefab),
                            new TestGamePrefab("alpha_item", null),
                        }));

                Assert.That(exception.MissingGamePrefabs.Select(gamePrefab => gamePrefab.id),
                    Is.EqualTo(new[] { "alpha_item", "zeta_item" }));
                Assert.That(exception.Message, Does.Contain("2 registered Game Prefab(s)"));
                Assert.That(exception.Message, Does.Contain("'alpha_item'"));
                Assert.That(exception.Message, Does.Contain("'zeta_item'"));
                Assert.That(exception.Message,
                    Does.Contain("Assign every IPrefabProvider.Prefab before startup."));
            }
            finally
            {
                Object.DestroyImmediate(validPrefab);
            }
        }

        [Test]
        public void DestroyedPrefabReferenceIsReported()
        {
            var destroyedPrefab = new GameObject("Destroyed Prefab");
            var gamePrefab = new TestGamePrefab("destroyed_item", destroyedPrefab);
            Object.DestroyImmediate(destroyedPrefab);

            var exception = Assert.Throws<MissingGamePrefabReferencesException>(
                () => GamePrefabReferenceValidator.Validate(
                    new IGamePrefab[] { gamePrefab }));

            Assert.That(exception.MissingGamePrefabs.Single(), Is.SameAs(gamePrefab));
        }
    }
}
