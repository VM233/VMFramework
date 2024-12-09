using UnityEngine;

namespace VMFramework.GameEvents
{
    public interface IVector2InputGameEvent : IInputGameEvent<Vector2>
    {
        public Vector2 value { get; }
    }
}