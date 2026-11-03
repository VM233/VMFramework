using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Effects
{
    public class Effect : ControllerGameItem, IEffect, INaturalReturnGameItem
    {
        [TitleGroup(ComponentNames.CONFIG)]
        public bool enableLogging;

        public event IGameItemDelayedReturnDispatcher.DelayedPreReturnHandler OnDelayedPreReturn;

        private EffectParticleSystems particles;
        private EffectAttachment attachment;
        private CancellationTokenSource playbackCancellation;
        private IGameItemManager returnOwner;
        private Transform container;
        private bool stopping;

        public bool IsStopping => stopping;

        protected override void OnCreate()
        {
            base.OnCreate();
            particles = new EffectParticleSystems(transform);
            returnOwner = GameItemManager.Instance;
            container = EffectSpawner.Instance.Container;
        }

        protected override void OnGet()
        {
            stopping = false;
            base.OnGet();
            particles.Play();
            if (particles.HasSystems)
            {
                playbackCancellation = new CancellationTokenSource();
                ObservePlayback(playbackCancellation.Token).Forget();
            }
        }

        public void SetSpawnTransform(Vector3 position, bool isWorldSpace, Transform parent, bool alignToParent = false)
        {
            attachment?.Dispose();
            attachment = null;
            if (particles.HasSystems && parent != null)
            {
                transform.SetParent(container, false);
                transform.SetPositionAndRotation(isWorldSpace ? position : parent.TransformPoint(position),
                    alignToParent ? parent.rotation : Quaternion.identity);
                transform.localScale = alignToParent ? parent.lossyScale : Vector3.one;
                attachment = new EffectAttachment(parent, transform, Stop);
            }
            else
            {
                transform.SetParent(parent != null ? parent : container, !alignToParent);
                if (alignToParent)
                {
                    transform.localRotation = Quaternion.identity;
                    transform.localScale = Vector3.one;
                }
                if (isWorldSpace) transform.position = position;
                else transform.localPosition = position;
            }
        }

        private void LateUpdate() => attachment?.Apply();

        private async UniTask ObservePlayback(CancellationToken cancellation)
        {
            // Observe after Unity's native VFXUpdate, including initial events and Stop.
            while (!await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, cancellation)
                       .SuppressCancellationThrow())
            {
                if (particles.IsAlive()) continue;
                returnOwner.Return(this);
                return;
            }
        }

        public void Stop()
        {
            if (particles.HasSystems)
            {
                DelayedReturn();
                return;
            }
            if (IsDestroyed) return;
            DelayedReturn();
            returnOwner.Return(this);
        }

        public bool TryBeginNaturalReturn()
        {
            if (!particles.HasSystems) return false;
            Stop();
            return true;
        }

        public virtual void DelayedReturn()
        {
            if (stopping) return;
            stopping = true;
            attachment?.Dispose();
            attachment = null;
            if (particles.HasSystems) transform.SetParent(container, true);
            particles.StopEmission();
            OnDelayedPreReturn?.Invoke(this);
        }

        protected override void OnReturn()
        {
            ReleasePlayback();
            particles.Clear();
            base.OnReturn();
            transform.SetParent(container);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        private void ReleasePlayback()
        {
            attachment?.Dispose();
            attachment = null;
            if (playbackCancellation == null) return;
            playbackCancellation.Cancel();
            playbackCancellation.Dispose();
            playbackCancellation = null;
        }

        protected override void OnDestroy()
        {
            ReleasePlayback();
            particles?.Dispose();
            base.OnDestroy();
        }

        protected virtual void OnEnable()
        {
            if (enableLogging) Debug.Log($"Effect {name} enabled", this);
        }

        protected virtual void OnDisable()
        {
            if (enableLogging) Debug.Log($"Effect {name} disabled", this);
        }
    }
}
