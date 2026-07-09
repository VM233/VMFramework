using System;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.OdinExtensions;

namespace VMFramework.Parameters
{
    [Serializable]
    public class VectorLengthSourceConfig : IParameterSourceConfig<float>
    {
        public VectorLengthSource.Type type = VectorLengthSource.Type.Vector2;

        [ShowIf(nameof(type), VectorLengthSource.Type.Vector2)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<Vector2> vector2SourceConfig = new ParameterSourceConfig<Vector2>();

        [ShowIf(nameof(type), VectorLengthSource.Type.Vector3)]
        [IsNotNullOrEmpty]
        [SerializeReference]
        public IParameterSourceConfig<Vector3> vector3SourceConfig = new ParameterSourceConfig<Vector3>();

        public virtual IParameterSource<float> GetParameterSource()
        {
            if (type is VectorLengthSource.Type.Vector2)
            {
                return new VectorLengthSource(vector2SourceConfig.GetParameterSource());
            }

            if (type is VectorLengthSource.Type.Vector3)
            {
                return new VectorLengthSource(vector3SourceConfig.GetParameterSource());
            }

            throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}