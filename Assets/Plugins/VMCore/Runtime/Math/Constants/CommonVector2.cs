using UnityEngine;

namespace VMFramework.Core
{
    public static class CommonVector2
    {
        /// <summary>
        /// Shorthand for Vector2(-1, 1)
        /// </summary>
        public static readonly Vector2 upLeft = new(-1, 1);

        /// <summary>
        /// Shorthand for Vector2(1, 1)
        /// </summary>
        public static readonly Vector2 upRight = new(1, 1);
        
        /// <summary>
        /// Shorthand for Vector2(-1, -1)
        /// </summary>
        public static readonly Vector2 downLeft = new(-1, -1);
        
        /// <summary>
        /// Shorthand for Vector2(1, -1)
        /// </summary>
        public static readonly Vector2 downRight = new(1, -1);
    }
}