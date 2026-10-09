# Precise 2D mouse candidates

`ColliderMouseEventTrigger` accepts an optional serialized `pointFilter2D`.
Without a filter, its native Collider2D supplies the exact mouse geometry.
With a filter, `ColliderMouseEventManager` first obtains native candidates, then
passes the world-space point to `ContainsPoint2D` before priority and stay-event
publication. A rejected coarse candidate cannot cover a lower-priority target.

`SpritePhysicsPointGeometry.Capture` snapshots Sprite physics contours at binding time.
The caller owns the snapshot and replaces it when the displayed Sprite changes.
Its bounds can size a single coarse BoxCollider2D; callers transform the world
point into that collider's local coordinates for exact contour selection.
The query performs no allocations and uses no physics simulation.

The focused `SpritePhysicsPointGeometryTests` compare multi-path selection to a
native PolygonCollider2D under rotations, reflection and nonuniform scale, and
verify that replacing source contours does not mutate an already published snapshot.
