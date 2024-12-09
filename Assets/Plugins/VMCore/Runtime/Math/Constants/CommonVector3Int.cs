using UnityEngine;

namespace VMFramework.Core
{
    public static class CommonVector3Int
    {
        public static readonly Vector3Int maxValue = new(int.MaxValue, int.MaxValue, int.MaxValue);
        
        public static readonly Vector3Int minValue = new(int.MinValue, int.MinValue, int.MinValue);
        
        public static readonly Vector3Int upLeft = new(-1, 1, 0);
        
        public static readonly Vector3Int upRight = new(1, 1, 0);
        
        public static readonly Vector3Int downLeft = new(-1, -1, 0);
        
        public static readonly Vector3Int downRight = new(1, -1, 0);
        
        public static readonly Vector3Int forwardLeft = new(-1, 0, 1);
        
        public static readonly Vector3Int forwardRight = new(1, 0, 1);
        
        public static readonly Vector3Int backLeft = new(-1, 0, -1);
        
        public static readonly Vector3Int backRight = new(1, 0, -1);
        
        public static readonly Vector3Int upForward = new(0, 1, 1);
        
        public static readonly Vector3Int upBack = new(0, 1, -1);
        
        public static readonly Vector3Int downForward = new(0, -1, 1);
        
        public static readonly Vector3Int downBack = new(0, -1, -1);
    }
}