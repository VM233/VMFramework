using System.Runtime.CompilerServices;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

namespace VMFramework.Core
{
    public partial class Math
    {
        #region Abs

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Abs(this int num)
        {
            return Mathf.Abs(num);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Abs(this float num)
        {
            return Mathf.Abs(num);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Abs(this double num)
        {
            return System.Math.Abs(num);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Abs(this Vector4 vector)
        {
            vector.x = vector.x.Abs();
            vector.y = vector.y.Abs();
            vector.z = vector.z.Abs();
            vector.w = vector.w.Abs();
            return vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int Abs(this Vector3Int vector)
        {
            vector.x = vector.x.Abs();
            vector.y = vector.y.Abs();
            vector.z = vector.z.Abs();
            return vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Abs(this Vector3 vector)
        {
            vector.x = vector.x.Abs();
            vector.y = vector.y.Abs();
            vector.z = vector.z.Abs();
            return vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int Abs(this Vector2Int vector)
        {
            vector.x = vector.x.Abs();
            vector.y = vector.y.Abs();
            return vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Abs(this Vector2 vector)
        {
            vector.x = vector.x.Abs();
            vector.y = vector.y.Abs();
            return vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color Abs(this Color color)
        {
            color.r = color.r.Abs();
            color.g = color.g.Abs();
            color.b = color.b.Abs();
            color.a = color.a.Abs();
            return color;
        }

        #endregion

        #region Abs XYZW

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 AbsX(this Vector2 vector)
        {
            vector.x = vector.x.Abs();
            return vector;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 AbsY(this Vector2 vector)
        {
            vector.y = vector.y.Abs();
            return vector;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 AbsX(this Vector3 vector)
        {
            vector.x = vector.x.Abs();
            return vector;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 AbsY(this Vector3 vector)
        {
            vector.y = vector.y.Abs();
            return vector;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 AbsZ(this Vector3 vector)
        {
            vector.z = vector.z.Abs();
            return vector;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int AbsX(this Vector2Int vector)
        {
            vector.x = vector.x.Abs();
            return vector;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2Int AbsY(this Vector2Int vector)
        {
            vector.y = vector.y.Abs();
            return vector;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int AbsX(this Vector3Int vector)
        {
            vector.x = vector.x.Abs();
            return vector;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int AbsY(this Vector3Int vector)
        {
            vector.y = vector.y.Abs();
            return vector;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int AbsZ(this Vector3Int vector)
        {
            vector.z = vector.z.Abs();
            return vector;
        }

        #endregion

        #region L1Norm

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float L1Norm(this Vector2 a)
        {
            return a.Abs().Sum();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float L1Norm(this Vector3 a)
        {
            return a.Abs().Sum();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float L1Norm(this Vector4 a)
        {
            return a.Abs().Sum();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int L1Norm(this Vector2Int a)
        {
            return a.Abs().Sum();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int L1Norm(this Vector3Int a)
        {
            return a.Abs().Sum();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float L1Norm(this Color a)
        {
            return a.Abs().Sum();
        }

        #endregion

        #region L2Norm

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float L2Norm(this Vector2 a)
        {
            return a.magnitude;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float L2Norm(this Vector3 a)
        {
            return a.magnitude;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float L2Norm(this Vector4 a)
        {
            return a.magnitude;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float L2Norm(this Vector2Int a)
        {
            return a.magnitude;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float L2Norm(this Vector3Int a)
        {
            return a.magnitude;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float L2Norm(this Color a)
        {
            return ((Vector4)a).magnitude;
        }

        #endregion

        #region Infinity Norm

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InfinityNorm(this Vector2 a)
        {
            return a.Abs().Max();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InfinityNorm(this Vector3 a)
        {
            return a.Abs().Max();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InfinityNorm(this Vector4 a)
        {
            return a.Abs().Max();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int InfinityNorm(this Vector2Int a)
        {
            return a.Abs().Max();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int InfinityNorm(this Vector3Int a)
        {
            return a.Abs().Max();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InfinityNorm(this Color a)
        {
            return a.Abs().Max();
        }

        #endregion

        #region Norm

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Norm(this Vector2 a, double p)
        {
            var vector = Vector<float>.Build.DenseOfArray(new[] { a.x, a.y });

            return (float)vector.Norm(p);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Norm(this Vector3 a, double p)
        {
            var vector = Vector<float>.Build.DenseOfArray(new[] { a.x, a.y, a.z });

            return (float)vector.Norm(p);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Norm(this Vector4 a, double p)
        {
            var vector = Vector<float>.Build.DenseOfArray(new[] { a.x, a.y, a.z, a.w });

            return (float)vector.Norm(p);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Norm(this Vector2Int a, double p)
        {
            var vector = Vector<int>.Build.DenseOfArray(new[] { a.x, a.y });

            return (float)vector.Norm(p);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Norm(this Vector3Int a, double p)
        {
            var vector = Vector<int>.Build.DenseOfArray(new[] { a.x, a.y, a.z });

            return (float)vector.Norm(p);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Norm(this Color a, double p)
        {
            var vector = Vector<float>.Build.DenseOfArray(new[] { a.r, a.g, a.b, a.a });

            return (float)vector.Norm(p);
        }

        #endregion
    }
}