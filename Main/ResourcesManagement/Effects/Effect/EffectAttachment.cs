using System;
using UnityEngine;
using VMFramework.Core.Pools;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Effects
{
    internal sealed class EffectAttachment : IDisposable
    {
        private readonly Transform source;
        private readonly Transform target;
        private readonly Action stop;
        private readonly IControllerGameItem sourceOwner;
        private Matrix4x4 previousWorldToLocal;
        private Quaternion previousRotation;
        private Vector3 previousScale;
        private bool disposed;

        internal EffectAttachment(Transform source, Transform target, Action stop)
        {
            this.source = source;
            this.target = target;
            this.stop = stop;
            CapturePose();
            sourceOwner = source.GetComponentInParent<IControllerGameItem>();
            if (sourceOwner != null) sourceOwner.OnReturnEvent += OnSourceReturn;
        }

        internal void Apply()
        {
            if (disposed) return;
            if (source == null || !source.gameObject.activeInHierarchy)
            {
                stop();
                return;
            }
            var scale = source.lossyScale;
            target.position = source.localToWorldMatrix.MultiplyPoint3x4(
                previousWorldToLocal.MultiplyPoint3x4(target.position));
            target.rotation = source.rotation * Quaternion.Inverse(previousRotation) * target.rotation;
            target.localScale = Vector3.Scale(target.localScale,
                new Vector3(scale.x / previousScale.x, scale.y / previousScale.y, scale.z / previousScale.z));
            CapturePose();
        }

        private void CapturePose()
        {
            previousWorldToLocal = source.worldToLocalMatrix;
            previousRotation = source.rotation;
            previousScale = source.lossyScale;
            if (previousScale.x == 0 || previousScale.y == 0 || previousScale.z == 0)
                throw new InvalidOperationException("A particle effect attachment requires nonzero source scale.");
        }

        private void OnSourceReturn(IReturnEventProvider provider)
        {
            if (disposed) return;
            Apply();
            stop();
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            if (sourceOwner != null) sourceOwner.OnReturnEvent -= OnSourceReturn;
        }
    }
}
