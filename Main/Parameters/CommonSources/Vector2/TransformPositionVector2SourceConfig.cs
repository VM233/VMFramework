using System;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;

namespace VMFramework.Parameters
{
    [Serializable]
    public class TransformPositionVector2SourceConfig : IParameterSourceConfig<Vector2>, IParameterSource<Vector2>
    {
        [Required]
        public Transform transform;

        public IParameterSource<Vector2> GetParameterSource()
        {
            return this;
        }

        public virtual bool TryGetValue(ref Vector2 value)
        {
            value = transform.position.XY();
            return true;
        }
    }
}