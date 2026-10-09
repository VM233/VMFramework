using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using VMFramework.GameEvents;
using Object = UnityEngine.Object;

namespace VMFramework.Tests
{
    public sealed class SpritePhysicsPointGeometryTests
    {
        [TestCase(0f, 1f, 1f)]
        [TestCase(37f, 1f, 1f)]
        [TestCase(-71f, -1f, 1f)]
        [TestCase(163f, 1f, -1f)]
        [TestCase(18f, 0.5f, 1.7f)]
        [TestCase(-29f, -0.8f, -1.3f)]
        public void ComplexContoursAgreeWithNativeCollider(float rotation, float xScale, float yScale)
        {
            var paths = new List<Vector2[]>
            {
                Circle(65, Vector2.zero, 1f, false),
                Circle(49, new Vector2(-0.35f, 0), 0.18f, true),
                Circle(52, new Vector2(0.35f, 0), 0.18f, true)
            };
            var texture = new Texture2D(256, 256);
            var sprite = CreateSprite(texture);
            var owner = new GameObject("Contour Native Control");
            try
            {
                OverrideLocalContours(sprite, paths);
                var geometry = new SpritePhysicsPointGeometry(sprite);
                var native = owner.AddComponent<PolygonCollider2D>();
                native.isTrigger = true;
                native.pathCount = paths.Count;
                for (int i = 0; i < paths.Count; i++) native.SetPath(i, paths[i]);
                owner.transform.SetPositionAndRotation(new Vector3(4.1f, -2.7f, 0),
                    Quaternion.Euler(0, 0, rotation));
                owner.transform.localScale = new Vector3(xScale, yScale, 1);
                Physics2D.SyncTransforms();

                Assert.That(geometry.PathCount, Is.EqualTo(paths.Count));
                Assert.That(geometry.LocalBounds.size.x, Is.GreaterThan(1.9f));
                for (int y = 0; y < 64; y++)
                for (int x = 0; x < 64; x++)
                {
                    Vector2 local = new(-1.2f + (x + 0.5f) * 2.4f / 64,
                        -1.2f + (y + 0.5f) * 2.4f / 64);
                    Vector2 world = owner.transform.TransformPoint(local);
                    Vector2 queryLocal = owner.transform.InverseTransformPoint(world);
                    Assert.That(geometry.ContainsLocalPoint(queryLocal), Is.EqualTo(native.OverlapPoint(world)),
                        $"Local point {queryLocal}, pose {rotation}/{xScale}/{yScale}");
                }
            }
            finally
            {
                Object.DestroyImmediate(owner);
                Object.DestroyImmediate(sprite);
                Object.DestroyImmediate(texture);
            }
        }

        [Test]
        public void BindingOwnsSnapshotUntilExplicitReplacement()
        {
            var texture = new Texture2D(32, 32);
            var sprite = CreateSprite(texture);
            try
            {
                OverrideLocalContours(sprite, new List<Vector2[]> { Circle(8, Vector2.zero, 0.1f, false) });
                var first = new SpritePhysicsPointGeometry(sprite);
                OverrideLocalContours(sprite, new List<Vector2[]> { Circle(8, new Vector2(0.2f, 0), 0.1f, false) });
                var second = new SpritePhysicsPointGeometry(sprite);
                Assert.That(first.ContainsLocalPoint(Vector2.zero), Is.True);
                Assert.That(first.ContainsLocalPoint(new Vector2(0.2f, 0)), Is.False);
                Assert.That(second.ContainsLocalPoint(Vector2.zero), Is.False);
                Assert.That(second.ContainsLocalPoint(new Vector2(0.2f, 0)), Is.True);
            }
            finally
            {
                Object.DestroyImmediate(sprite);
                Object.DestroyImmediate(texture);
            }
        }

        private static Sprite CreateSprite(Texture2D texture) => Sprite.Create(texture,
            new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f, 100, 0,
            SpriteMeshType.FullRect, Vector4.zero, true);

        private static void OverrideLocalContours(Sprite sprite, List<Vector2[]> localPaths)
        {
            var rectPaths = new List<Vector2[]>(localPaths.Count);
            foreach (Vector2[] localPath in localPaths)
            {
                var rectPath = new Vector2[localPath.Length];
                for (int i = 0; i < localPath.Length; i++)
                    rectPath[i] = localPath[i] * sprite.pixelsPerUnit + sprite.pivot;
                rectPaths.Add(rectPath);
            }
            sprite.OverridePhysicsShape(rectPaths);
        }

        private static Vector2[] Circle(int count, Vector2 center, float radius, bool reverse)
        {
            var points = new Vector2[count];
            for (int i = 0; i < count; i++)
            {
                float angle = (reverse ? -1 : 1) * 2 * Mathf.PI * i / count;
                points[i] = center + radius * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            }
            return points;
        }
    }
}
