using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VMFramework.Core.Pools;
using VMFramework.Effects;
using VMFramework.GameLogicArchitecture;
using Object = UnityEngine.Object;

namespace VMFramework.Tests
{
    public sealed class EffectParticleLifetimeTests
    {
        private readonly List<GameObject> objects = new();
        private GameItemManager manager;
        private Effect effect;
        private ParticleSystem shortParticles, longParticles;
        private string identity;
        private int returnCount, stopCount;
        private const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;

        [SetUp]
        public void CreateNativeRentalOwners()
        {
            returnCount = stopCount = 0;
            Assert.That(PlayerLoopHelper.IsInjectedUniTaskPlayerLoop(), Is.True,
                "The native fixture requires UniTask's production PlayerLoop injection.");
            identity = "particle_lifetime_" + Guid.NewGuid().ToString("N");
            Assert.That(GamePrefabManager.RegisterGamePrefab(new EffectConfig { id = identity }), Is.True);
            manager = Native("Particle pool owner").AddComponent<GameItemManager>();
            var spawner = Native("Effect spawn owner").AddComponent<EffectSpawner>();
            typeof(EffectSpawner).GetField("<Container>k__BackingField", Private).SetValue(spawner,
                Native("Independent effect container").transform);
            typeof(GameItemManager).GetField("createGameItemHandler", Private).SetValue(manager,
                new Func<string, IGameItem>(_ => CreateEffect()));
            effect = manager.Get<Effect>(identity);
            effect.OnReturnEvent += _ => returnCount++;
            effect.OnDelayedPreReturn += _ => stopCount++;
            Emit();
        }

        private GameObject Native(string name)
        {
            var result = new GameObject(name);
            objects.Add(result);
            return result;
        }

        private Effect CreateEffect()
        {
            var root = Native("Native particle rental");
            root.SetActive(false);
            shortParticles = AddParticles(root, 0.1f);
            var child = Native("Long child particles");
            child.transform.SetParent(root.transform);
            longParticles = AddParticles(child, 2f);
            return root.AddComponent<Effect>();
        }

        private static ParticleSystem AddParticles(GameObject host, float lifetime)
        {
            var particles = host.AddComponent<ParticleSystem>();
            var main = particles.main;
            main.playOnAwake = true;
            main.loop = true;
            main.startLifetime = lifetime;
            main.startSpeed = 0;
            main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            var emission = particles.emission;
            emission.rateOverTime = 0;
            return particles;
        }

        private void Emit()
        {
            shortParticles.Emit(1);
            longParticles.Emit(1);
        }

        [TearDown]
        public void RetireNativeOwners()
        {
            if (effect != null && !effect.IsDestroyed) manager.Return(effect);
            for (int index = objects.Count - 1; index >= 0; index--)
                if (objects[index] != null) Object.DestroyImmediate(objects[index]);
            objects.Clear();
            GamePrefabManager.UnregisterGamePrefab(identity);
            LogAssert.NoUnexpectedReceived();
        }

        [UnityTest]
        public IEnumerator StopRetainsBothTailsAndWaitsForTheLastChild()
        {
            effect.Stop();
            effect.Stop();
            Assert.That(stopCount, Is.EqualTo(1));
            Assert.That(shortParticles.particleCount, Is.EqualTo(1));
            Assert.That(longParticles.particleCount, Is.EqualTo(1));
            Assert.That(shortParticles.isEmitting || longParticles.isEmitting, Is.False);
            AdvanceStopped(shortParticles, 0.2f);
            yield return null;
            Assert.That(effect.IsDestroyed, Is.False, "The root must not outrun its living child.");
            Assert.That(longParticles.particleCount, Is.EqualTo(1));
            AdvanceStopped(longParticles, 3);
            yield return AwaitReturn();
            Assert.That(returnCount, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator ForcedReturnCannotLetTheOldObserverRetireTheNextRental()
        {
            effect.Stop();
            manager.Return(effect);
            var next = manager.Get<Effect>(identity);
            Assert.That(next, Is.SameAs(effect));
            Assert.That(next.IsStopping, Is.False);
            Assert.That(shortParticles.particleCount + longParticles.particleCount, Is.Zero);
            Emit();
            yield return null;
            yield return null;
            Assert.That(next.IsDestroyed, Is.False);
            Assert.That(longParticles.particleCount, Is.EqualTo(1));
            next.Stop();
            AdvanceStopped(shortParticles, 3);
            AdvanceStopped(longParticles, 3);
            yield return AwaitReturn();
            Assert.That(returnCount, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator ParentDisableKeepsTheNativeEffectAndItsParticlesActive()
        {
            var parent = Native("Disabled visual owner");
            parent.transform.position = new Vector3(3, 4, 0);
            parent.transform.localScale = new Vector3(-2, 2, 1);
            effect.SetSpawnTransform(Vector3.zero, false, parent.transform);
            Assert.That(effect.transform.IsChildOf(parent.transform), Is.False);
            Assert.That(effect.transform.lossyScale, Is.EqualTo(Vector3.one),
                "The default attachment must preserve the previous world scale.");
            parent.SetActive(false);
            yield return null;
            Assert.That(effect.IsStopping, Is.True);
            Assert.That(effect.gameObject.activeInHierarchy, Is.True);
            Assert.That(longParticles.particleCount, Is.EqualTo(1));
            Assert.That(effect.transform.position, Is.EqualTo(new Vector3(3, 4, 0)));
            AdvanceStopped(shortParticles, 3);
            AdvanceStopped(longParticles, 3);
            yield return AwaitReturn();
        }

        [UnityTest]
        public IEnumerator ParentDestructionFreezesTheLastWorldPoseAndReleasesItsTail()
        {
            var parent = Native("Destroyed visual owner");
            effect.SetSpawnTransform(new Vector3(1, 2, 0), false, parent.transform);
            parent.transform.position = new Vector3(5, 6, 0);
            yield return null;
            var pose = effect.transform.position;
            Object.Destroy(parent);
            yield return null;
            yield return null;
            Assert.That(effect.IsStopping, Is.True);
            Assert.That(effect.transform.position, Is.EqualTo(pose));
            Assert.That(longParticles.particleCount, Is.EqualTo(1));
            AdvanceStopped(shortParticles, 3);
            AdvanceStopped(longParticles, 3);
            yield return AwaitReturn();
        }

        [UnityTest]
        public IEnumerator NativeDestructionCancelsAnOutstandingCompletionObservation()
        {
            effect.Stop();
            Object.Destroy(effect.gameObject);
            yield return null;
            yield return null;
            Assert.That(effect == null, Is.True);
            Assert.That(returnCount, Is.Zero);
        }

        [UnityTest]
        public IEnumerator ZeroParticlesInAPausedLoopDoNotProveNativeCompletion()
        {
            effect.Stop();
            shortParticles.Simulate(3, false, false, false);
            longParticles.Simulate(3, false, false, false);
            Assert.That(shortParticles.particleCount + longParticles.particleCount, Is.Zero);
            Assert.That(shortParticles.isPaused && longParticles.isPaused, Is.True);
            Assert.That(shortParticles.IsAlive(false) && longParticles.IsAlive(false), Is.True);
            yield return null;
            yield return null;
            Assert.That(effect.IsDestroyed, Is.False,
                "A paused looping system can still produce future particles.");
            shortParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            longParticles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            yield return AwaitReturn();
        }

        [UnityTest]
        public IEnumerator ExplicitAlignmentRetainsLocalIdentityAndTracksItsParent()
        {
            var parent = Native("Aligned particle anchor");
            parent.transform.SetPositionAndRotation(new Vector3(3, 4, 0), Quaternion.Euler(0, 0, 30));
            parent.transform.localScale = new Vector3(2, 2, 1);
            effect.SetSpawnTransform(Vector3.zero, false, parent.transform, alignToParent: true);
            Assert.That(effect.transform.position, Is.EqualTo(parent.transform.position));
            Assert.That(Quaternion.Angle(effect.transform.rotation, parent.transform.rotation), Is.LessThan(0.001f));
            Assert.That(effect.transform.lossyScale, Is.EqualTo(parent.transform.lossyScale));
            parent.transform.position = new Vector3(5, 6, 0);
            yield return null;
            Assert.That(effect.transform.position, Is.EqualTo(parent.transform.position));
            effect.Stop();
            AdvanceStopped(shortParticles, 3);
            AdvanceStopped(longParticles, 3);
            yield return AwaitReturn();
        }

        private static void AdvanceStopped(ParticleSystem particles, float seconds)
        {
            // Simulate ends in Pause, which retains a looping system's future playback.
            particles.Simulate(seconds, false, false, false);
            particles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }

        private IEnumerator AwaitReturn()
        {
            for (int frame = 0; frame < 8 && !effect.IsDestroyed; frame++) yield return null;
            Assert.That(effect.IsDestroyed, Is.True,
                $"Native completion failed: injected={PlayerLoopHelper.IsInjectedUniTaskPlayerLoop()}, " +
                $"root=({shortParticles.particleCount},alive={shortParticles.IsAlive(false)},playing={shortParticles.isPlaying},paused={shortParticles.isPaused},stopped={shortParticles.isStopped}), " +
                $"child=({longParticles.particleCount},alive={longParticles.IsAlive(false)},playing={longParticles.isPlaying},paused={longParticles.isPaused},stopped={longParticles.isStopped}).");
        }
    }
}
