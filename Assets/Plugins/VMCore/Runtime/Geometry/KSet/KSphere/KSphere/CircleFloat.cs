using UnityEngine;
using System;
using Random = System.Random;

namespace VMFramework.Core
{
    public struct CircleFloat : IKSphere<Vector2, float>
    {
        public readonly Vector2 center;

        public readonly float radius;

        #region Interface Implementation

        Vector2 IKSphere<Vector2, float>.Center
        {
            get => center;
            init => center = value;
        }

        float IKSphere<Vector2, float>.Radius
        {
            get => radius;
            init => radius = value;
        }

        #endregion

        #region Constructors

        public CircleFloat(Vector2 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public CircleFloat(float x, float y, float radius)
        {
            this.center = new Vector2(x, y);
            this.radius = radius;
        }

        public CircleFloat(float radius)
        {
            this.center = Vector2.zero;
            this.radius = radius;
        }

        #endregion

        public bool Contains(Vector2 pos)
        {
            return pos.EuclideanDistance(center) <= radius;
        }

        public Vector2 GetRelativePos(Vector2 pos)
        {
            return pos - center;
        }

        public Vector2 Clamp(Vector2 pos)
        {
            return pos.ClampMaxMagnitude(radius);
        }

        public Vector2 GetRandomPointInside(Random random)
        {
            return random.PointInsideCircle(radius) + center;
        }

        public Vector2 GetRandomPointOnSurface(Random random)
        {
            return random.PointOnCircle(radius) + center;
        }
    }
}