using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace VMFramework.Effects
{
    internal sealed class EffectParticleSystems : IDisposable
    {
        private readonly ParticleSystem[] particles;
        private readonly VisualEffect[] visuals;
        private readonly int[][] spawnSystems;
        private readonly VFXSpawnerState[] spawnStates;

        internal bool HasSystems => particles.Length + visuals.Length > 0;

        internal EffectParticleSystems(Transform root)
        {
            particles = root.GetComponentsInChildren<ParticleSystem>(true);
            visuals = root.GetComponentsInChildren<VisualEffect>(true);
            spawnSystems = new int[visuals.Length][];
            spawnStates = new VFXSpawnerState[visuals.Length];
            var names = new List<string>();
            for (int index = 0; index < visuals.Length; index++)
            {
                visuals[index].GetSpawnSystemNames(names);
                var ids = new int[names.Count];
                for (int name = 0; name < names.Count; name++)
                    ids[name] = Shader.PropertyToID(names[name]);
                spawnSystems[index] = ids;
                spawnStates[index] = new VFXSpawnerState();
                names.Clear();
            }
        }

        internal void Play()
        {
            foreach (var particle in particles)
            {
                particle.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
                if (particle.main.playOnAwake && particle.gameObject.activeInHierarchy)
                    particle.Play(false);
            }
            foreach (var visual in visuals)
                if (visual.gameObject.activeInHierarchy) visual.Reinit();
        }

        internal void StopEmission()
        {
            foreach (var particle in particles)
                particle.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            foreach (var visual in visuals) visual.Stop();
        }

        internal bool IsAlive()
        {
            foreach (var particle in particles)
                if (particle.IsAlive(false)) return true;
            for (int index = 0; index < visuals.Length; index++)
            {
                var visual = visuals[index];
                if (visual.HasAnySystemAwake() || visual.aliveParticleCount > 0) return true;
                foreach (int system in spawnSystems[index])
                {
                    visual.GetSpawnSystemInfo(system, spawnStates[index]);
                    if (spawnStates[index].playing) return true;
                }
            }
            return false;
        }

        internal void Clear()
        {
            foreach (var particle in particles)
                particle.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            // Disabling the owning Effect releases native VFX simulation.
            foreach (var visual in visuals) visual.Stop();
        }

        public void Dispose()
        {
            foreach (var state in spawnStates) state.Dispose();
        }
    }
}
