using System;
using UnityEngine;

namespace VMFramework.Parameters
{
    public struct VectorLengthSource : IParameterSource<float>
    {
        public enum Type
        {
            Vector2,
            Vector3,
        }

        public Type type;
        public IParameterSource<Vector2> vector2Source;
        public IParameterSource<Vector3> vector3Source;

        public VectorLengthSource(IParameterSource<Vector2> vector2Source)
        {
            this.type = Type.Vector2;
            this.vector2Source = vector2Source;
            this.vector3Source = null;
        }

        public VectorLengthSource(IParameterSource<Vector3> vector3Source)
        {
            this.type = Type.Vector3;
            this.vector2Source = null;
            this.vector3Source = vector3Source;
        }

        public bool TryGetValue(ref float value)
        {
            if (type is Type.Vector2)
            {
                if (vector2Source.TryGetValue(out var vector2Value))
                {
                    value = vector2Value.magnitude;
                    return true;
                }

                return false;
            }

            if (type is Type.Vector3)
            {
                if (vector3Source.TryGetValue(out var vector3Value))
                {
                    value = vector3Value.magnitude;
                    return true;
                }

                return false;
            }

            throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}