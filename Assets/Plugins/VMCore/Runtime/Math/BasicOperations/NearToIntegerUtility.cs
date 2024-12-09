using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core
{
    public static class NearToIntegerUtility
    {
        #region Round

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Round(this float value) => Mathf.RoundToInt(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Round(this double value) => (int)System.Math.Round(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Round(this Vector2 vector) => new(vector.x.Round(), vector.y.Round());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int Round(this Vector3 vector) =>
            new(vector.x.Round(), vector.y.Round(), vector.z.Round());

        #endregion

        #region Ceiling

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Ceiling(this float value) => Mathf.CeilToInt(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Ceiling(this double value) => (int)System.Math.Ceiling(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Ceiling(this Vector2 vector) => new(vector.x.Ceiling(), vector.y.Ceiling());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int Ceiling(this Vector3 vector) =>
            new(vector.x.Ceiling(), vector.y.Ceiling(), vector.z.Ceiling());

        #endregion

        #region Floor

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Floor(this float value) => Mathf.FloorToInt(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Floor(this double value) => (int)System.Math.Floor(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Floor(this Vector2 vector) => new(vector.x.Floor(), vector.y.Floor());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int Floor(this Vector3 vector) =>
            new(vector.x.Floor(), vector.y.Floor(), vector.z.Floor());

        #endregion

        #region Floor To Zero

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FloorToZero(this float value)
        {
            if (value < 0)
            {
                return value.Ceiling();
            }

            return value.Floor();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FloorToZero(this double value)
        {
            if (value < 0)
            {
                return value.Ceiling();
            }

            return value.Floor();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int FloorToZero(this Vector2 vector) =>
            new(vector.x.FloorToZero(), vector.y.FloorToZero());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int FloorToZero(this Vector3 vector) =>
            new(vector.x.FloorToZero(), vector.y.FloorToZero(), vector.z.FloorToZero());

        #endregion

        #region Ceiling To Zero

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CeilingToZero(this float value)
        {
            if (value < 0)
            {
                return value.Floor();
            }

            return value.Ceiling();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CeilingToZero(this double value)
        {
            if (value < 0)
            {
                return value.Floor();
            }

            return value.Ceiling();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int CeilingToZero(this Vector2 vector) =>
            new(vector.x.CeilingToZero(), vector.y.CeilingToZero());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int CeilingToZero(this Vector3 vector) =>
            new(vector.x.CeilingToZero(), vector.y.CeilingToZero(), vector.z.CeilingToZero());

        #endregion
    }
}