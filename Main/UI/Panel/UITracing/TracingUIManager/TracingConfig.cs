using UnityEngine;

namespace VMFramework.UI
{
    public readonly struct TracingConfig
    {
        public readonly TracingType tracingType;
        public readonly Vector3 tracingWorldPosition;
        public readonly Transform tracingTransform;
        public readonly Vector3 tracingOffset;
        public readonly int maxTracingCount;
        public readonly bool hasMaxTracingCount;

        public TracingConfig(Vector3 worldPosition, int maxTracingCount = -1)
        {
            tracingType = TracingType.WorldPosition;
            tracingWorldPosition = worldPosition;
            tracingTransform = null;
            tracingOffset = Vector3.zero;
            this.maxTracingCount = maxTracingCount;
            hasMaxTracingCount = maxTracingCount >= 0;
        }

        public TracingConfig(Vector3 worldPosition, bool persistentTracing)
        {
            tracingType = TracingType.WorldPosition;
            tracingWorldPosition = worldPosition;
            tracingTransform = null;
            tracingOffset = Vector3.zero;

            if (persistentTracing)
            {
                maxTracingCount = -1;
                hasMaxTracingCount = false;
            }
            else
            {
                maxTracingCount = 1;
                hasMaxTracingCount = true;
            }
        }

        public TracingConfig(Transform transform, Vector3 tracingOffset = default, int maxTracingCount = -1)
        {
            tracingType = TracingType.Transform;
            tracingWorldPosition = Vector3.zero;
            tracingTransform = transform;
            this.tracingOffset = tracingOffset;
            this.maxTracingCount = maxTracingCount;
            hasMaxTracingCount = maxTracingCount >= 0;
        }

        public TracingConfig(Transform transform, bool persistentTracing, Vector3 tracingOffset = default)
        {
            tracingType = TracingType.Transform;
            tracingWorldPosition = Vector3.zero;
            tracingTransform = transform;
            this.tracingOffset = tracingOffset;

            if (persistentTracing)
            {
                maxTracingCount = -1;
                hasMaxTracingCount = false;
            }
            else
            {
                maxTracingCount = 1;
                hasMaxTracingCount = true;
            }
        }

        public TracingConfig(int maxTracingCount = -1)
        {
            tracingType = TracingType.MousePosition;
            tracingWorldPosition = Vector3.zero;
            tracingTransform = null;
            tracingOffset = Vector3.zero;
            this.maxTracingCount = maxTracingCount;
            hasMaxTracingCount = maxTracingCount >= 0;
        }

        public TracingConfig(bool persistentTracing)
        {
            tracingType = TracingType.MousePosition;
            tracingWorldPosition = Vector3.zero;
            tracingTransform = null;
            tracingOffset = Vector3.zero;

            if (persistentTracing)
            {
                maxTracingCount = -1;
                hasMaxTracingCount = false;
            }
            else
            {
                maxTracingCount = 1;
                hasMaxTracingCount = true;
            }
        }

        public static implicit operator TracingConfig(Vector3 worldPosition)
        {
            return new TracingConfig(worldPosition);
        }

        public static implicit operator TracingConfig(Transform transform)
        {
            return new TracingConfig(transform);
        }

        public static implicit operator TracingConfig(bool persistentTracing)
        {
            return new TracingConfig(persistentTracing);
        }
    }
}