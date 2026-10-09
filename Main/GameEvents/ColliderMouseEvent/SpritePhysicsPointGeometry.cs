using System;
using System.Collections.Generic;
using UnityEngine;

namespace VMFramework.GameEvents
{
    /// <summary>An owned snapshot of a Sprite's local physics contours.</summary>
    public sealed class SpritePhysicsPointGeometry
    {
        private readonly Vector2[] edgeStarts;
        private readonly Vector2[] edgeEnds;

        public int PathCount { get; }
        public Bounds LocalBounds { get; }

        public static SpritePhysicsPointGeometry Capture(Sprite sprite)
        {
            int pathCount = sprite.GetPhysicsShapeCount();
            if (pathCount == 0)
                throw new InvalidOperationException(
                    $"Sprite '{sprite.name}' has no physics shape for point selection.");
            var paths = new Vector2[pathCount][];
            var points = new List<Vector2>();
            for (int pathIndex = 0; pathIndex < pathCount; pathIndex++)
            {
                points.Clear();
                sprite.GetPhysicsShape(pathIndex, points);
                paths[pathIndex] = points.ToArray();
            }
            return new SpritePhysicsPointGeometry(paths);
        }

        public SpritePhysicsPointGeometry(IReadOnlyList<Vector2[]> paths)
        {
            PathCount = paths.Count;
            var starts = new List<Vector2>();
            var ends = new List<Vector2>();
            Bounds bounds = default;
            bool firstPoint = true;
            for (int pathIndex = 0; pathIndex < PathCount; pathIndex++)
            {
                Vector2[] points = paths[pathIndex];
                if (points.Length < 3)
                    throw new InvalidOperationException(
                        $"Point-selection contour {pathIndex} has fewer than three vertices.");
                Vector2 previous = points[^1];
                foreach (Vector2 current in points)
                {
                    starts.Add(previous);
                    ends.Add(current);
                    if (firstPoint)
                    {
                        bounds = new Bounds(current, Vector3.zero);
                        firstPoint = false;
                    }
                    else bounds.Encapsulate(current);
                    previous = current;
                }
            }
            edgeStarts = starts.ToArray();
            edgeEnds = ends.ToArray();
            LocalBounds = bounds;
        }

        public bool ContainsLocalPoint(Vector2 point)
        {
            bool inside = false;
            for (int i = 0; i < edgeStarts.Length; i++)
            {
                Vector2 start = edgeStarts[i];
                Vector2 end = edgeEnds[i];
                double cross = ((double)point.x - start.x) * ((double)end.y - start.y) -
                               ((double)point.y - start.y) * ((double)end.x - start.x);
                if (cross == 0 && point.x >= Math.Min(start.x, end.x) &&
                    point.x <= Math.Max(start.x, end.x) && point.y >= Math.Min(start.y, end.y) &&
                    point.y <= Math.Max(start.y, end.y))
                    return true;
                if ((start.y > point.y) != (end.y > point.y) &&
                    point.x < start.x + ((double)point.y - start.y) * (end.x - start.x) /
                    ((double)end.y - start.y))
                    inside = !inside;
            }
            return inside;
        }
    }
}
