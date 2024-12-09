using UnityEngine;

namespace VMFramework.Core
{
    public struct Ray3DInfo
    {
        public Vector3 origin;
        public Vector3 direction;
        public float distance;
        
        public Ray Ray => new(origin, direction);
        
        public Ray3DInfo(Vector3 origin, Vector3 direction, float distance = 1f)
        {
            this.origin = origin;
            this.direction = direction;
            this.distance = distance;
        }
    }
}