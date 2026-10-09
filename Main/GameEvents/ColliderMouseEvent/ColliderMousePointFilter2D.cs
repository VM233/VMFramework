using UnityEngine;

namespace VMFramework.GameEvents
{
    /// <summary>Refines native 2D mouse candidates in world coordinates.</summary>
    public abstract class ColliderMousePointFilter2D : MonoBehaviour
    {
        public abstract bool ContainsPoint2D(Vector2 worldPoint);
    }
}
