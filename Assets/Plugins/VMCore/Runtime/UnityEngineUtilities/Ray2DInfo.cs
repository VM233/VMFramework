using UnityEngine;

namespace VMFramework.Core
{
    public struct Ray2DInfo
    {
        public Vector2 origin;
        public Vector2 direction;
        public float distance;
        
        public Ray2D Ray => new(origin, direction);
        
        public Ray2DInfo(Vector2 origin, Vector2 direction, float distance = 1f)
        {
            this.origin = origin;
            this.direction = direction;
            this.distance = distance;
        }
    }
}