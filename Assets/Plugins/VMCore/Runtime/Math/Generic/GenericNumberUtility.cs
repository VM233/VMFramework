using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VMFramework.Core.Generic
{
    public static class GenericNumberUtility
    {
        public static readonly HashSet<Type> numberTypes = new()
        {
            typeof(int),
            typeof(float),
            typeof(double)
        };

        public static readonly HashSet<Type> vectorTypes = new()
        {
            typeof(Vector2),
            typeof(Vector3),
            typeof(Vector2Int),
            typeof(Vector3Int),
            typeof(Vector4),
            typeof(Color)
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNumber(this Type type) => numberTypes.Contains(type);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsVector(this Type type) => vectorTypes.Contains(type);

        public static T ConvertNumber<T>(object value) =>
            (T)Convert.ChangeType(value, typeof(T));

        #region Clamp

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ClampMin<T>(this T a, T min)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => ClampUtility.ClampMin(a.ConvertTo<int>(), min.ConvertTo<int>()).ConvertTo<T>(),
                float => ClampUtility.ClampMin(a.ConvertTo<float>(), min.ConvertTo<float>()).ConvertTo<T>(),
                double => ClampUtility.ClampMin(a.ConvertTo<double>(), min.ConvertTo<double>()).ConvertTo<T>(),
                Vector2 => ClampUtility.ClampMin(a.ConvertTo<Vector2>(), min.ConvertTo<Vector2>()).ConvertTo<T>(),
                Vector3 => ClampUtility.ClampMin(a.ConvertTo<Vector3>(), min.ConvertTo<Vector3>()).ConvertTo<T>(),
                Vector4 => ClampUtility.ClampMin(a.ConvertTo<Vector4>(), min.ConvertTo<Vector4>()).ConvertTo<T>(),
                Vector2Int => ClampUtility.ClampMin(a.ConvertTo<Vector2Int>(), min.ConvertTo<Vector2Int>())
                    .ConvertTo<T>(),
                Vector3Int => ClampUtility.ClampMin(a.ConvertTo<Vector3Int>(), min.ConvertTo<Vector3Int>())
                    .ConvertTo<T>(),
                Color => ClampUtility.ClampMin(a.ConvertTo<Color>(), min.ConvertTo<Color>()).ConvertTo<T>(),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ClampMax<T>(this T a, T min)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => ClampUtility.ClampMax(a.ConvertTo<int>(), min.ConvertTo<int>()).ConvertTo<T>(),
                float => ClampUtility.ClampMax(a.ConvertTo<float>(), min.ConvertTo<float>()).ConvertTo<T>(),
                double => ClampUtility.ClampMax(a.ConvertTo<double>(), min.ConvertTo<double>()).ConvertTo<T>(),
                Vector2 => ClampUtility.ClampMax(a.ConvertTo<Vector2>(), min.ConvertTo<Vector2>()).ConvertTo<T>(),
                Vector3 => ClampUtility.ClampMax(a.ConvertTo<Vector3>(), min.ConvertTo<Vector3>()).ConvertTo<T>(),
                Vector4 => ClampUtility.ClampMax(a.ConvertTo<Vector4>(), min.ConvertTo<Vector4>()).ConvertTo<T>(),
                Vector2Int => ClampUtility.ClampMax(a.ConvertTo<Vector2Int>(), min.ConvertTo<Vector2Int>())
                    .ConvertTo<T>(),
                Vector3Int => ClampUtility.ClampMax(a.ConvertTo<Vector3Int>(), min.ConvertTo<Vector3Int>())
                    .ConvertTo<T>(),
                Color => ClampUtility.ClampMax(a.ConvertTo<Color>(), min.ConvertTo<Color>()).ConvertTo<T>(),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ClampMin<T>(this T a, double min)
            where T : IEquatable<T>
        {
            return a switch
            {
                int intValue => ClampUtility.ClampMin(intValue, min).ConvertTo<T>(),
                float floatValue => ClampUtility.ClampMin(floatValue, min).ConvertTo<T>(),
                double doubleValue => ClampUtility.ClampMin(doubleValue, min).ConvertTo<T>(),
                Vector2 vector => ClampUtility.ClampMin(vector, (float)min).ConvertTo<T>(),
                Vector3 vector => ClampUtility.ClampMin(vector, (float)min).ConvertTo<T>(),
                Vector4 vector => ClampUtility.ClampMin(vector, (float)min).ConvertTo<T>(),
                Vector2Int vector => ClampUtility.ClampMin(vector, NearToIntegerUtility.Ceiling(min)).ConvertTo<T>(),
                Vector3Int vector => ClampUtility.ClampMin(vector, NearToIntegerUtility.Ceiling(min)).ConvertTo<T>(),
                Color color => ClampUtility.ClampMin(color, (float)min).ConvertTo<T>(),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ClampMax<T>(this T a, double min)
            where T : IEquatable<T>
        {
            return a switch
            {
                int intValue => ClampUtility.ClampMax(intValue, min).ConvertTo<T>(),
                float floatValue => ClampUtility.ClampMax(floatValue, min).ConvertTo<T>(),
                double doubleValue => ClampUtility.ClampMax(doubleValue, min).ConvertTo<T>(),
                Vector2 vector => ClampUtility.ClampMax(vector, (float)min).ConvertTo<T>(),
                Vector3 vector => ClampUtility.ClampMax(vector, (float)min).ConvertTo<T>(),
                Vector4 vector => ClampUtility.ClampMax(vector, (float)min).ConvertTo<T>(),
                Vector2Int vector => ClampUtility.ClampMax(vector, NearToIntegerUtility.Floor(min)).ConvertTo<T>(),
                Vector3Int vector => ClampUtility.ClampMax(vector, NearToIntegerUtility.Floor(min)).ConvertTo<T>(),
                Color color => ClampUtility.ClampMax(color, (float)min).ConvertTo<T>(),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Clamp<T>(this T a, T min, T max)
            where T : IEquatable<T>
        {
            return a.ClampMin(min).ClampMax(max);
        }

        #endregion

        #region Compare

        #region AllNumber

        /// <summary>
        /// return true if num is in range [min, max]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="num"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllNumberBetweenInclusive<T>(this T num, T min, T max)
            where T : IEquatable<T>
        {
            return num.AllNumberAboveOrEqual(min) && num.AllNumberBelowOrEqual(max);
        }

        /// <summary>
        /// return true if num is in range (min, max)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="num"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllNumberBetweenExclusive<T>(this T num, T min, T max)
            where T : IEquatable<T>
        {
            return num.AllNumberAbove(min) && num.AllNumberBelow(max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllNumberBelow<T>(this T a, T comparison)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => Math.AllNumberBelow(a.ConvertTo<int>(), comparison.ConvertTo<int>()),
                float => Math.AllNumberBelow(a.ConvertTo<float>(), comparison.ConvertTo<float>()),
                double => Math.AllNumberBelow(a.ConvertTo<double>(), comparison.ConvertTo<double>()),
                Vector2 => Math.AllNumberBelow(a.ConvertTo<Vector2>(), comparison.ConvertTo<Vector2>()),
                Vector3 => Math.AllNumberBelow(a.ConvertTo<Vector3>(), comparison.ConvertTo<Vector3>()),
                Vector4 => Math.AllNumberBelow(a.ConvertTo<Vector4>(), comparison.ConvertTo<Vector4>()),
                Vector2Int => Math.AllNumberBelow(a.ConvertTo<Vector2Int>(), comparison.ConvertTo<Vector2Int>()),
                Vector3Int => Math.AllNumberBelow(a.ConvertTo<Vector3Int>(), comparison.ConvertTo<Vector3Int>()),
                Color => Math.AllNumberBelow(a.ConvertTo<Color>(), comparison.ConvertTo<Color>()),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllNumberAbove<T>(this T a, T comparison)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => Math.AllNumberAbove(a.ConvertTo<int>(), comparison.ConvertTo<int>()),
                float => Math.AllNumberAbove(a.ConvertTo<float>(), comparison.ConvertTo<float>()),
                double => Math.AllNumberAbove(a.ConvertTo<double>(), comparison.ConvertTo<double>()),
                Vector2 => Math.AllNumberAbove(a.ConvertTo<Vector2>(), comparison.ConvertTo<Vector2>()),
                Vector3 => Math.AllNumberAbove(a.ConvertTo<Vector3>(), comparison.ConvertTo<Vector3>()),
                Vector4 => Math.AllNumberAbove(a.ConvertTo<Vector4>(), comparison.ConvertTo<Vector4>()),
                Vector2Int => Math.AllNumberAbove(a.ConvertTo<Vector2Int>(), comparison.ConvertTo<Vector2Int>()),
                Vector3Int => Math.AllNumberAbove(a.ConvertTo<Vector3Int>(), comparison.ConvertTo<Vector3Int>()),
                Color => Math.AllNumberAbove(a.ConvertTo<Color>(), comparison.ConvertTo<Color>()),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllNumberBelowOrEqual<T>(this T a, T comparison)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => Math.AllNumberBelowOrEqual(a.ConvertTo<int>(), comparison.ConvertTo<int>()),
                float => Math.AllNumberBelowOrEqual(a.ConvertTo<float>(), comparison.ConvertTo<float>()),
                double => Math.AllNumberBelowOrEqual(a.ConvertTo<double>(), comparison.ConvertTo<double>()),
                Vector2 => Math.AllNumberBelowOrEqual(a.ConvertTo<Vector2>(), comparison.ConvertTo<Vector2>()),
                Vector3 => Math.AllNumberBelowOrEqual(a.ConvertTo<Vector3>(), comparison.ConvertTo<Vector3>()),
                Vector4 => Math.AllNumberBelowOrEqual(a.ConvertTo<Vector4>(), comparison.ConvertTo<Vector4>()),
                Vector2Int => Math.AllNumberBelowOrEqual(a.ConvertTo<Vector2Int>(), comparison.ConvertTo<Vector2Int>()),
                Vector3Int => Math.AllNumberBelowOrEqual(a.ConvertTo<Vector3Int>(), comparison.ConvertTo<Vector3Int>()),
                Color => Math.AllNumberBelowOrEqual(a.ConvertTo<Color>(), comparison.ConvertTo<Color>()),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AllNumberAboveOrEqual<T>(this T a, T comparison)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => Math.AllNumberAboveOrEqual(a.ConvertTo<int>(), comparison.ConvertTo<int>()),
                float => Math.AllNumberAboveOrEqual(a.ConvertTo<float>(), comparison.ConvertTo<float>()),
                double => Math.AllNumberAboveOrEqual(a.ConvertTo<double>(), comparison.ConvertTo<double>()),
                Vector2 => Math.AllNumberAboveOrEqual(a.ConvertTo<Vector2>(), comparison.ConvertTo<Vector2>()),
                Vector3 => Math.AllNumberAboveOrEqual(a.ConvertTo<Vector3>(), comparison.ConvertTo<Vector3>()),
                Vector4 => Math.AllNumberAboveOrEqual(a.ConvertTo<Vector4>(), comparison.ConvertTo<Vector4>()),
                Vector2Int => Math.AllNumberAboveOrEqual(a.ConvertTo<Vector2Int>(), comparison.ConvertTo<Vector2Int>()),
                Vector3Int => Math.AllNumberAboveOrEqual(a.ConvertTo<Vector3Int>(), comparison.ConvertTo<Vector3Int>()),
                Color => Math.AllNumberAboveOrEqual(a.ConvertTo<Color>(), comparison.ConvertTo<Color>()),
                _ => throw new ArgumentException()
            };
        }

        #endregion

        #region AnyNumber

        /// <summary>
        /// return true if num is in range [min, max]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="num"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyNumberBetweenInclusive<T>(this T num, T min, T max)
            where T : IEquatable<T>
        {
            return num.AnyNumberAboveOrEqual(min) && num.AnyNumberBelowOrEqual(max);
        }

        /// <summary>
        /// return true if num is in range (min, max)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="num"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyNumberBetweenExclusive<T>(this T num, T min, T max)
            where T : IEquatable<T>
        {
            return num.AnyNumberAbove(min) && num.AnyNumberBelow(max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyNumberBelow<T>(this T a, T comparison)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => Math.AnyNumberBelow(a.ConvertTo<int>(), comparison.ConvertTo<int>()),
                float => Math.AnyNumberBelow(a.ConvertTo<float>(), comparison.ConvertTo<float>()),
                double => Math.AnyNumberBelow(a.ConvertTo<double>(), comparison.ConvertTo<double>()),
                Vector2 => Math.AnyNumberBelow(a.ConvertTo<Vector2>(), comparison.ConvertTo<Vector2>()),
                Vector3 => Math.AnyNumberBelow(a.ConvertTo<Vector3>(), comparison.ConvertTo<Vector3>()),
                Vector4 => Math.AnyNumberBelow(a.ConvertTo<Vector4>(), comparison.ConvertTo<Vector4>()),
                Vector2Int => Math.AnyNumberBelow(a.ConvertTo<Vector2Int>(), comparison.ConvertTo<Vector2Int>()),
                Vector3Int => Math.AnyNumberBelow(a.ConvertTo<Vector3Int>(), comparison.ConvertTo<Vector3Int>()),
                Color => Math.AnyNumberBelow(a.ConvertTo<Color>(), comparison.ConvertTo<Color>()),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyNumberAbove<T>(this T a, T comparison)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => Math.AnyNumberAbove(a.ConvertTo<int>(), comparison.ConvertTo<int>()),
                float => Math.AnyNumberAbove(a.ConvertTo<float>(), comparison.ConvertTo<float>()),
                double => Math.AnyNumberAbove(a.ConvertTo<double>(), comparison.ConvertTo<double>()),
                Vector2 => Math.AnyNumberAbove(a.ConvertTo<Vector2>(), comparison.ConvertTo<Vector2>()),
                Vector3 => Math.AnyNumberAbove(a.ConvertTo<Vector3>(), comparison.ConvertTo<Vector3>()),
                Vector4 => Math.AnyNumberAbove(a.ConvertTo<Vector4>(), comparison.ConvertTo<Vector4>()),
                Vector2Int => Math.AnyNumberAbove(a.ConvertTo<Vector2Int>(), comparison.ConvertTo<Vector2Int>()),
                Vector3Int => Math.AnyNumberAbove(a.ConvertTo<Vector3Int>(), comparison.ConvertTo<Vector3Int>()),
                Color => Math.AnyNumberAbove(a.ConvertTo<Color>(), comparison.ConvertTo<Color>()),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyNumberBelowOrEqual<T>(this T a, T comparison)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => Math.AnyNumberBelowOrEqual(a.ConvertTo<int>(), comparison.ConvertTo<int>()),
                float => Math.AnyNumberBelowOrEqual(a.ConvertTo<float>(), comparison.ConvertTo<float>()),
                double => Math.AnyNumberBelowOrEqual(a.ConvertTo<double>(), comparison.ConvertTo<double>()),
                Vector2 => Math.AnyNumberBelowOrEqual(a.ConvertTo<Vector2>(), comparison.ConvertTo<Vector2>()),
                Vector3 => Math.AnyNumberBelowOrEqual(a.ConvertTo<Vector3>(), comparison.ConvertTo<Vector3>()),
                Vector4 => Math.AnyNumberBelowOrEqual(a.ConvertTo<Vector4>(), comparison.ConvertTo<Vector4>()),
                Vector2Int => Math.AnyNumberBelowOrEqual(a.ConvertTo<Vector2Int>(), comparison.ConvertTo<Vector2Int>()),
                Vector3Int => Math.AnyNumberBelowOrEqual(a.ConvertTo<Vector3Int>(), comparison.ConvertTo<Vector3Int>()),
                Color => Math.AnyNumberBelowOrEqual(a.ConvertTo<Color>(), comparison.ConvertTo<Color>()),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyNumberAboveOrEqual<T>(this T a, T comparison)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => Math.AnyNumberAboveOrEqual(a.ConvertTo<int>(), comparison.ConvertTo<int>()),
                float => Math.AnyNumberAboveOrEqual(a.ConvertTo<float>(), comparison.ConvertTo<float>()),
                double => Math.AnyNumberAboveOrEqual(a.ConvertTo<double>(), comparison.ConvertTo<double>()),
                Vector2 => Math.AnyNumberAboveOrEqual(a.ConvertTo<Vector2>(), comparison.ConvertTo<Vector2>()),
                Vector3 => Math.AnyNumberAboveOrEqual(a.ConvertTo<Vector3>(), comparison.ConvertTo<Vector3>()),
                Vector4 => Math.AnyNumberAboveOrEqual(a.ConvertTo<Vector4>(), comparison.ConvertTo<Vector4>()),
                Vector2Int => Math.AnyNumberAboveOrEqual(a.ConvertTo<Vector2Int>(), comparison.ConvertTo<Vector2Int>()),
                Vector3Int => Math.AnyNumberAboveOrEqual(a.ConvertTo<Vector3Int>(), comparison.ConvertTo<Vector3Int>()),
                Color => Math.AnyNumberAboveOrEqual(a.ConvertTo<Color>(), comparison.ConvertTo<Color>()),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyNumberBelowOrEqual<T>(this T a, double comparison)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => Math.AnyNumberBelowOrEqual(a.ConvertTo<int>(), NearToIntegerUtility.Floor(comparison)),
                float => Math.AnyNumberBelowOrEqual(a.ConvertTo<float>(), comparison.F()),
                double => Math.AnyNumberBelowOrEqual(a.ConvertTo<double>(), comparison),
                Vector2 => Math.AnyNumberBelowOrEqual(a.ConvertTo<Vector2>(), comparison.F()),
                Vector3 => Math.AnyNumberBelowOrEqual(a.ConvertTo<Vector3>(), comparison.F()),
                Vector4 => Math.AnyNumberBelowOrEqual(a.ConvertTo<Vector4>(), comparison.F()),
                Vector2Int => Math.AnyNumberBelowOrEqual(a.ConvertTo<Vector2Int>(), NearToIntegerUtility.Floor(comparison)),
                Vector3Int => Math.AnyNumberBelowOrEqual(a.ConvertTo<Vector3Int>(), NearToIntegerUtility.Floor(comparison)),
                Color => Math.AnyNumberBelowOrEqual(a.ConvertTo<Color>(), comparison.F()),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyNumberBelow<T>(this T a, double comparison)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => Math.AnyNumberBelow(a.ConvertTo<int>(), NearToIntegerUtility.Floor(comparison)),
                float => Math.AnyNumberBelow(a.ConvertTo<float>(), comparison.F()),
                double => Math.AnyNumberBelow(a.ConvertTo<double>(), comparison),
                Vector2 => Math.AnyNumberBelow(a.ConvertTo<Vector2>(), comparison.F()),
                Vector3 => Math.AnyNumberBelow(a.ConvertTo<Vector3>(), comparison.F()),
                Vector4 => Math.AnyNumberBelow(a.ConvertTo<Vector4>(), comparison.F()),
                Vector2Int => Math.AnyNumberBelow(a.ConvertTo<Vector2Int>(), NearToIntegerUtility.Floor(comparison)),
                Vector3Int => Math.AnyNumberBelow(a.ConvertTo<Vector3Int>(), NearToIntegerUtility.Floor(comparison)),
                Color => Math.AnyNumberBelow(a.ConvertTo<Color>(), comparison.F()),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyNumberAboveOrEqual<T>(this T a, double comparison)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => Math.AnyNumberAboveOrEqual(a.ConvertTo<int>(), NearToIntegerUtility.Ceiling(comparison)),
                float => Math.AnyNumberAboveOrEqual(a.ConvertTo<float>(), comparison.F()),
                double => Math.AnyNumberAboveOrEqual(a.ConvertTo<double>(), comparison),
                Vector2 => Math.AnyNumberAboveOrEqual(a.ConvertTo<Vector2>(), comparison.F()),
                Vector3 => Math.AnyNumberAboveOrEqual(a.ConvertTo<Vector3>(), comparison.F()),
                Vector4 => Math.AnyNumberAboveOrEqual(a.ConvertTo<Vector4>(), comparison.F()),
                Vector2Int => Math.AnyNumberAboveOrEqual(a.ConvertTo<Vector2Int>(), NearToIntegerUtility.Ceiling(comparison)),
                Vector3Int => Math.AnyNumberAboveOrEqual(a.ConvertTo<Vector3Int>(), NearToIntegerUtility.Ceiling(comparison)),
                Color => Math.AnyNumberAboveOrEqual(a.ConvertTo<Color>(), comparison.F()),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AnyNumberAbove<T>(this T a, double comparison)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => Math.AnyNumberAbove(a.ConvertTo<int>(), NearToIntegerUtility.Ceiling(comparison)),
                float => Math.AnyNumberAbove(a.ConvertTo<float>(), comparison.F()),
                double => Math.AnyNumberAbove(a.ConvertTo<double>(), comparison),
                Vector2 => Math.AnyNumberAbove(a.ConvertTo<Vector2>(), comparison.F()),
                Vector3 => Math.AnyNumberAbove(a.ConvertTo<Vector3>(), comparison.F()),
                Vector4 => Math.AnyNumberAbove(a.ConvertTo<Vector4>(), comparison.F()),
                Vector2Int => Math.AnyNumberAbove(a.ConvertTo<Vector2Int>(), NearToIntegerUtility.Ceiling(comparison)),
                Vector3Int => Math.AnyNumberAbove(a.ConvertTo<Vector3Int>(), NearToIntegerUtility.Ceiling(comparison)),
                Color => Math.AnyNumberAbove(a.ConvertTo<Color>(), comparison.F()),
                _ => throw new ArgumentException()
            };
        }


        #endregion

        #endregion

        // #region Round
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static T Round<T>(this T a)
        //     where T : IEquatable<T>
        // {
        //     return a switch
        //     {
        //         int => a.ConvertTo<T>(),
        //         float => NearToIntegerUtility.Round(a.ConvertTo<float>()).ConvertTo<T>(),
        //         double => NearToIntegerUtility.Round(a.ConvertTo<double>()).ConvertTo<T>(),
        //         Vector2 => NearToIntegerUtility.Round(a.ConvertTo<Vector2>()).ConvertTo<T>(),
        //         Vector3 => NearToIntegerUtility.Round(a.ConvertTo<Vector3>()).ConvertTo<T>(),
        //         Vector4 => NearToIntegerUtility.Round(a.ConvertTo<Vector4>()).ConvertTo<T>(),
        //         Vector2Int => a.ConvertTo<T>(),
        //         Vector3Int => a.ConvertTo<T>(),
        //         Color => a.ConvertTo<T>(),
        //         _ => throw new ArgumentException()
        //     };
        // }
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static T Ceiling<T>(this T a)
        //     where T : IEquatable<T>
        // {
        //     return a switch
        //     {
        //         int => a.ConvertTo<T>(),
        //         float => NearToIntegerUtility.Ceiling(a.ConvertTo<float>()).ConvertTo<T>(),
        //         double => NearToIntegerUtility.Ceiling(a.ConvertTo<double>()).ConvertTo<T>(),
        //         Vector2 => NearToIntegerUtility.Ceiling(a.ConvertTo<Vector2>()).ConvertTo<T>(),
        //         Vector3 => NearToIntegerUtility.Ceiling(a.ConvertTo<Vector3>()).ConvertTo<T>(),
        //         Vector4 => NearToIntegerUtility.Ceiling(a.ConvertTo<Vector4>()).ConvertTo<T>(),
        //         Vector2Int => a.ConvertTo<T>(),
        //         Vector3Int => a.ConvertTo<T>(),
        //         Color => a.ConvertTo<T>(),
        //         _ => throw new ArgumentException()
        //     };
        // }
        //
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static T Floor<T>(this T a)
        //     where T : IEquatable<T>
        // {
        //     return a switch
        //     {
        //         int => a.ConvertTo<T>(),
        //         float => NearToIntegerUtility.Floor(a.ConvertTo<float>()).ConvertTo<T>(),
        //         double => NearToIntegerUtility.Floor(a.ConvertTo<double>()).ConvertTo<T>(),
        //         Vector2 => NearToIntegerUtility.Floor(a.ConvertTo<Vector2>()).ConvertTo<T>(),
        //         Vector3 => NearToIntegerUtility.Floor(a.ConvertTo<Vector3>()).ConvertTo<T>(),
        //         Vector4 => NearToIntegerUtility.Floor(a.ConvertTo<Vector4>()).ConvertTo<T>(),
        //         Vector2Int => a.ConvertTo<T>(),
        //         Vector3Int => a.ConvertTo<T>(),
        //         Color => a.ConvertTo<T>(),
        //         _ => throw new ArgumentException()
        //     };
        // }
        //
        // #endregion

        #region + - * / ^

        #region Add

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Add<T>(this T a, T b)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => (a.ConvertTo<int>() + b.ConvertTo<int>()).ConvertTo<T>(),
                float => (a.ConvertTo<float>() + b.ConvertTo<float>()).ConvertTo<T>(),
                double => (a.ConvertTo<double>() + b.ConvertTo<double>()).ConvertTo<T>(),
                Vector2 => (a.ConvertTo<Vector2>() + b.ConvertTo<Vector2>()).ConvertTo<T>(),
                Vector3 => (a.ConvertTo<Vector3>() + b.ConvertTo<Vector3>()).ConvertTo<T>(),
                Vector4 => (a.ConvertTo<Vector4>() + b.ConvertTo<Vector4>()).ConvertTo<T>(),
                Vector2Int => (a.ConvertTo<Vector2Int>() + b.ConvertTo<Vector2Int>()).ConvertTo<T>(),
                Vector3Int => (a.ConvertTo<Vector3Int>() + b.ConvertTo<Vector3Int>()).ConvertTo<T>(),
                Color => (a.ConvertTo<Color>() + b.ConvertTo<Color>()).ConvertTo<T>(),
                _ => throw new ArgumentException()
            };
        }

        #endregion

        #region Substract

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Subtract<T>(this T a, T b)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => (a.ConvertTo<int>() - b.ConvertTo<int>()).ConvertTo<T>(),
                float => (a.ConvertTo<float>() - b.ConvertTo<float>()).ConvertTo<T>(),
                double => (a.ConvertTo<double>() - b.ConvertTo<double>()).ConvertTo<T>(),
                Vector2 => (a.ConvertTo<Vector2>() - b.ConvertTo<Vector2>()).ConvertTo<T>(),
                Vector3 => (a.ConvertTo<Vector3>() - b.ConvertTo<Vector3>()).ConvertTo<T>(),
                Vector4 => (a.ConvertTo<Vector4>() - b.ConvertTo<Vector4>()).ConvertTo<T>(),
                Vector2Int => (a.ConvertTo<Vector2Int>() - b.ConvertTo<Vector2Int>()).ConvertTo<T>(),
                Vector3Int => (a.ConvertTo<Vector3Int>() - b.ConvertTo<Vector3Int>()).ConvertTo<T>(),
                Color => (a.ConvertTo<Color>() - b.ConvertTo<Color>()).ConvertTo<T>(),
                _ => throw new ArgumentException()
            };
        }

        #endregion

        #region Negate

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Negate<T>(this T a)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => (-a.ConvertTo<int>()).ConvertTo<T>(),
                float => (-a.ConvertTo<float>()).ConvertTo<T>(),
                double => (-a.ConvertTo<double>()).ConvertTo<T>(),
                Vector2 => (-a.ConvertTo<Vector2>()).ConvertTo<T>(),
                Vector3 => (-a.ConvertTo<Vector3>()).ConvertTo<T>(),
                Vector4 => (-a.ConvertTo<Vector4>()).ConvertTo<T>(),
                Vector2Int => (-a.ConvertTo<Vector2Int>()).ConvertTo<T>(),
                Vector3Int => (-a.ConvertTo<Vector3Int>()).ConvertTo<T>(),
                Color => throw new ArgumentException(),
                _ => throw new ArgumentException(),
            };
        }

        #endregion

        #region Multiply

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Multiply<T>(this T a, double b)
            where T : IEquatable<T>
        {
            return a switch
            {
                int num => (num * b).ConvertTo<T>(),
                float num => (num * b).ConvertTo<T>(),
                double num => (num * b).ConvertTo<T>(),
                Vector2 vector => (vector * b.F()).ConvertTo<T>(),
                Vector3 vector => (vector * b.F()).ConvertTo<T>(),
                Vector4 vector => (vector * b.F()).ConvertTo<T>(),
                Vector2Int vector => ArithmeticUtility.Multiply(vector, b.F()).Round().ConvertTo<T>(),
                Vector3Int vector => ArithmeticUtility.Multiply(vector, b.F()).Round().ConvertTo<T>(),
                Color color => (color * b.F()).ConvertTo<T>(),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Multiply<T>(this T a, T b)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => (a.ConvertTo<int>() * b.ConvertTo<int>()).ConvertTo<T>(),
                float => (a.ConvertTo<float>() * b.ConvertTo<float>()).ConvertTo<T>(),
                double => (a.ConvertTo<double>() * b.ConvertTo<double>()).ConvertTo<T>(),
                Vector2 => ArithmeticUtility.Multiply(a.ConvertTo<Vector2>(), b.ConvertTo<Vector2>()).ConvertTo<T>(),
                Vector3 => ArithmeticUtility.Multiply(a.ConvertTo<Vector3>(), b.ConvertTo<Vector3>()).ConvertTo<T>(),
                Vector4 => ArithmeticUtility.Multiply(a.ConvertTo<Vector4>(), b.ConvertTo<Vector4>()).ConvertTo<T>(),
                Vector2Int => ArithmeticUtility.Multiply(a.ConvertTo<Vector2Int>(), b.ConvertTo<Vector2Int>()).ConvertTo<T>(),
                Vector3Int => ArithmeticUtility.Multiply(a.ConvertTo<Vector3Int>(), b.ConvertTo<Vector3Int>()).ConvertTo<T>(),
                Color => ArithmeticUtility.Multiply(a.ConvertTo<Color>(), b.ConvertTo<Color>()).ConvertTo<T>(),
                _ => throw new ArgumentException()
            };
        }

        #endregion

        #region Divide

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Divide<T>(this T dividend, double divisor)
            where T : IEquatable<T>
        {
            return dividend switch
            {
                int num => (num / divisor).Round().ConvertTo<T>(),
                float num => (num / divisor).F().ConvertTo<T>(),
                double num => (num / divisor).ConvertTo<T>(),
                Vector2 vector => ArithmeticUtility.Divide(vector, divisor.F()).ConvertTo<T>(),
                Vector3 vector => ArithmeticUtility.Divide(vector, divisor.F()).ConvertTo<T>(),
                Vector4 vector => ArithmeticUtility.Divide(vector, divisor.F()).ConvertTo<T>(),
                Vector2Int vector => ArithmeticUtility.Divide(vector, NearToIntegerUtility.Round(divisor)).ConvertTo<T>(),
                Vector3Int vector => ArithmeticUtility.Divide(vector, NearToIntegerUtility.Round(divisor)).ConvertTo<T>(),
                Color color => ArithmeticUtility.Divide(color, divisor.F()).ConvertTo<T>(),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Divide<T>(this T dividend, T divisor)
            where T : IEquatable<T>
        {
            return dividend switch
            {
                int => (dividend.ConvertTo<int>() / divisor.ConvertTo<int>()).ConvertTo<T>(),
                float => (dividend.ConvertTo<float>() / divisor.ConvertTo<float>()).ConvertTo<T>(),
                double => (dividend.ConvertTo<double>() / divisor.ConvertTo<double>()).ConvertTo<T>(),
                Vector2 => ArithmeticUtility.Divide(dividend.ConvertTo<Vector2>(), divisor.ConvertTo<Vector2>()).ConvertTo<T>(),
                Vector3 => ArithmeticUtility.Divide(dividend.ConvertTo<Vector3>(), divisor.ConvertTo<Vector3>()).ConvertTo<T>(),
                Vector4 => ArithmeticUtility.Divide(dividend.ConvertTo<Vector4>(), divisor.ConvertTo<Vector4>()).ConvertTo<T>(),
                Vector2Int => ArithmeticUtility.Divide(dividend.ConvertTo<Vector2Int>(), divisor.ConvertTo<Vector2Int>())
                    .ConvertTo<T>(),
                Vector3Int => ArithmeticUtility.Divide(dividend.ConvertTo<Vector3Int>(), divisor.ConvertTo<Vector3Int>())
                    .ConvertTo<T>(),
                Color => ArithmeticUtility.Divide(dividend.ConvertTo<Color>(), divisor.ConvertTo<Color>()).ConvertTo<T>(),
                _ => throw new ArgumentException()
            };
        }

        #endregion

        #endregion

        #region Min & Max

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Min<T>(this T a, T min)
            where T : struct, IEquatable<T>
        {
            return a switch
            {
                int => Math.Min(a.ConvertTo<int>(), min.ConvertTo<int>()).ConvertTo<T>(),
                float => Math.Min(a.ConvertTo<float>(), min.ConvertTo<float>()).ConvertTo<T>(),
                double => Math.Min(a.ConvertTo<double>(), min.ConvertTo<double>()).ConvertTo<T>(),
                Vector2 => Math.Min(a.ConvertTo<Vector2>(), min.ConvertTo<Vector2>()).ConvertTo<T>(),
                Vector3 => Math.Min(a.ConvertTo<Vector3>(), min.ConvertTo<Vector3>()).ConvertTo<T>(),
                Vector4 => Math.Min(a.ConvertTo<Vector4>(), min.ConvertTo<Vector4>()).ConvertTo<T>(),
                Vector2Int => Math.Min(a.ConvertTo<Vector2Int>(), min.ConvertTo<Vector2Int>()).ConvertTo<T>(),
                Vector3Int => Math.Min(a.ConvertTo<Vector3Int>(), min.ConvertTo<Vector3Int>()).ConvertTo<T>(),
                Color => Math.Min(a.ConvertTo<Color>(), min.ConvertTo<Color>()).ConvertTo<T>(),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Min<T>(this IEnumerable<T> enumerable)
            where T : struct, IEquatable<T>
        {
            return enumerable.Aggregate(Min);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Min<T, TResult>(this IEnumerable<T> enumerable, Func<T, TResult> selector)
            where TResult : struct, IEquatable<TResult>
        {
            return enumerable.Select(selector).Aggregate(Min);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MinOrDefault<T>(this IEnumerable<T> enumerable)
            where T : struct, IEquatable<T>
        {
            return MinOrDefault(enumerable, item => item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult MinOrDefault<T, TResult>(this IEnumerable<T> enumerable, Func<T, TResult> selector)
            where TResult : struct, IEquatable<TResult>
        {
            var list = enumerable.ToList();
            return list.Count == 0 ? default : list.Select(selector).Aggregate(Min);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Max<T>(this T a, T min)
            where T : struct, IEquatable<T>
        {
            return a switch
            {
                int => Math.Max(a.ConvertTo<int>(), min.ConvertTo<int>()).ConvertTo<T>(),
                float => Math.Max(a.ConvertTo<float>(), min.ConvertTo<float>()).ConvertTo<T>(),
                double => Math.Max(a.ConvertTo<double>(), min.ConvertTo<double>()).ConvertTo<T>(),
                Vector2 => Math.Max(a.ConvertTo<Vector2>(), min.ConvertTo<Vector2>()).ConvertTo<T>(),
                Vector3 => Math.Max(a.ConvertTo<Vector3>(), min.ConvertTo<Vector3>()).ConvertTo<T>(),
                Vector4 => Math.Max(a.ConvertTo<Vector4>(), min.ConvertTo<Vector4>()).ConvertTo<T>(),
                Vector2Int => Math.Max(a.ConvertTo<Vector2Int>(), min.ConvertTo<Vector2Int>()).ConvertTo<T>(),
                Vector3Int => Math.Max(a.ConvertTo<Vector3Int>(), min.ConvertTo<Vector3Int>()).ConvertTo<T>(),
                Color => Math.Max(a.ConvertTo<Color>(), min.ConvertTo<Color>()).ConvertTo<T>(),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Max<T>(this IEnumerable<T> enumerable)
            where T : struct, IEquatable<T>
        {
            return enumerable.Aggregate(Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Max<T, TResult>(this IEnumerable<T> enumerable, Func<T, TResult> selector)
            where TResult : struct, IEquatable<TResult>
        {
            return enumerable.Select(selector).Aggregate(Max);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MaxOrDefault<T>(this IEnumerable<T> enumerable)
            where T : struct, IEquatable<T>
        {
            return MaxOrDefault(enumerable, item => item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult MaxOrDefault<T, TResult>(this IEnumerable<T> enumerable, Func<T, TResult> selector)
            where TResult : struct, IEquatable<TResult>
        {
            var list = enumerable.ToList();
            return list.Count == 0 ? default : list.Select(selector).Aggregate(Max);
        }

        #endregion

        #region Abs

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Abs<T>(this T a)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => Math.Abs(a.ConvertTo<int>()).ConvertTo<T>(),
                float => Math.Abs(a.ConvertTo<float>()).ConvertTo<T>(),
                double => Math.Abs(a.ConvertTo<double>()).ConvertTo<T>(),
                Vector2 => Math.Abs(a.ConvertTo<Vector2>()).ConvertTo<T>(),
                Vector3 => Math.Abs(a.ConvertTo<Vector3>()).ConvertTo<T>(),
                Vector4 => Math.Abs(a.ConvertTo<Vector4>()).ConvertTo<T>(),
                Vector2Int => Math.Abs(a.ConvertTo<Vector2Int>()).ConvertTo<T>(),
                Vector3Int => Math.Abs(a.ConvertTo<Vector3Int>()).ConvertTo<T>(),
                Color => Math.Abs(a.ConvertTo<Color>()).ConvertTo<T>(),
                _ => throw new ArgumentException()
            };
        }

        #endregion

        #region Sum & Products

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Products<T>(this T a)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => a.ConvertTo<double>(),
                float => a.ConvertTo<double>(),
                double => a.ConvertTo<double>(),
                Vector2 => Math.Products(a.ConvertTo<Vector2>()),
                Vector3 => Math.Products(a.ConvertTo<Vector3>()),
                Vector4 => Math.Products(a.ConvertTo<Vector4>()),
                Vector2Int => Math.Products(a.ConvertTo<Vector2Int>()),
                Vector3Int => Math.Products(a.ConvertTo<Vector3Int>()),
                Color => Math.Products(a.ConvertTo<Color>()),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Sum<T>(this T a)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => a.ConvertTo<double>(),
                float => a.ConvertTo<double>(),
                double => a.ConvertTo<double>(),
                Vector2 => Math.Sum(a.ConvertTo<Vector2>()),
                Vector3 => Math.Sum(a.ConvertTo<Vector3>()),
                Vector4 => Math.Sum(a.ConvertTo<Vector4>()),
                Vector2Int => Math.Sum(a.ConvertTo<Vector2Int>()),
                Vector3Int => Math.Sum(a.ConvertTo<Vector3Int>()),
                Color => Math.Sum(a.ConvertTo<Color>()),
                _ => throw new ArgumentException()
            };
        }

        #endregion

        #region Norm

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double L1Norm<T>(this T a)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => a.ConvertTo<int>().Abs(),
                float => a.ConvertTo<float>().Abs(),
                double => a.ConvertTo<double>().Abs(),
                Vector2 => Math.L1Norm(a.ConvertTo<Vector2>()),
                Vector3 => Math.L1Norm(a.ConvertTo<Vector3>()),
                Vector4 => Math.L1Norm(a.ConvertTo<Vector4>()),
                Vector2Int => Math.L1Norm(a.ConvertTo<Vector2Int>()),
                Vector3Int => Math.L1Norm(a.ConvertTo<Vector3Int>()),
                Color => Math.L1Norm(a.ConvertTo<Color>()),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double L2Norm<T>(this T a)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => a.ConvertTo<int>().Abs(),
                float => a.ConvertTo<float>().Abs(),
                double => a.ConvertTo<double>().Abs(),
                Vector2 => Math.L2Norm(a.ConvertTo<Vector2>()),
                Vector3 => Math.L2Norm(a.ConvertTo<Vector3>()),
                Vector4 => Math.L2Norm(a.ConvertTo<Vector4>()),
                Vector2Int => Math.L2Norm(a.ConvertTo<Vector2Int>()),
                Vector3Int => Math.L2Norm(a.ConvertTo<Vector3Int>()),
                Color => Math.L2Norm(a.ConvertTo<Color>()),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double InfinityNorm<T>(this T a)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => a.ConvertTo<int>().Abs(),
                float => a.ConvertTo<float>().Abs(),
                double => a.ConvertTo<double>().Abs(),
                Vector2 => Math.InfinityNorm(a.ConvertTo<Vector2>()),
                Vector3 => Math.InfinityNorm(a.ConvertTo<Vector3>()),
                Vector4 => Math.InfinityNorm(a.ConvertTo<Vector4>()),
                Vector2Int => Math.InfinityNorm(a.ConvertTo<Vector2Int>()),
                Vector3Int => Math.InfinityNorm(a.ConvertTo<Vector3Int>()),
                Color => Math.InfinityNorm(a.ConvertTo<Color>()),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Norm<T>(this T a, double p)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => a.ConvertTo<int>().Abs(),
                float => a.ConvertTo<float>().Abs(),
                double => a.ConvertTo<double>().Abs(),
                Vector2 => Math.Norm(a.ConvertTo<Vector2>(), p),
                Vector3 => Math.Norm(a.ConvertTo<Vector3>(), p),
                Vector4 => Math.Norm(a.ConvertTo<Vector4>(), p),
                Vector2Int => Math.Norm(a.ConvertTo<Vector2Int>(), p),
                Vector3Int => Math.Norm(a.ConvertTo<Vector3Int>(), p),
                Color => Math.Norm(a.ConvertTo<Color>(), p),
                _ => throw new ArgumentException()
            };
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance<T>(this T a, T b)
            where T : IEquatable<T>
        {
            return a.Subtract(b).L2Norm();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Lerp<T>(this T from, T to, float t)
            where T : IEquatable<T>
        {
            return from switch
            {
                int => Math.Lerp(from.ConvertTo<int>(), to.ConvertTo<int>(), t).ConvertTo<T>(),
                float => Math.Lerp(from.ConvertTo<float>(), to.ConvertTo<float>(), t).ConvertTo<T>(),
                double => Math.Lerp(from.ConvertTo<double>(), to.ConvertTo<double>(), t).ConvertTo<T>(),
                Vector2 => Math.Lerp(from.ConvertTo<Vector2>(), to.ConvertTo<Vector2>(), t).ConvertTo<T>(),
                Vector3 => Math.Lerp(from.ConvertTo<Vector3>(), to.ConvertTo<Vector3>(), t).ConvertTo<T>(),
                Vector4 => Math.Lerp(from.ConvertTo<Vector4>(), to.ConvertTo<Vector4>(), t).ConvertTo<T>(),
                Vector2Int => Math.Lerp(from.ConvertTo<Vector2Int>(), to.ConvertTo<Vector2Int>(), t).ConvertTo<T>(),
                Vector3Int => Math.Lerp(from.ConvertTo<Vector3Int>(), to.ConvertTo<Vector3Int>(), t).ConvertTo<T>(),
                Color => Math.Lerp(from.ConvertTo<Color>(), to.ConvertTo<Color>(), t).ConvertTo<T>(),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Lerp<T>(this T from, T to, float t, float power)
            where T : IEquatable<T>
        {
            return from switch
            {
                int => Math.Lerp(from.ConvertTo<int>(), to.ConvertTo<int>(), t, power).ConvertTo<T>(),
                float => Math.Lerp(from.ConvertTo<float>(), to.ConvertTo<float>(), t, power).ConvertTo<T>(),
                double => Math.Lerp(from.ConvertTo<double>(), to.ConvertTo<double>(), t, power).ConvertTo<T>(),
                Vector2 => Math.Lerp(from.ConvertTo<Vector2>(), to.ConvertTo<Vector2>(), t, power).ConvertTo<T>(),
                Vector3 => Math.Lerp(from.ConvertTo<Vector3>(), to.ConvertTo<Vector3>(), t, power).ConvertTo<T>(),
                Vector4 => Math.Lerp(from.ConvertTo<Vector4>(), to.ConvertTo<Vector4>(), t, power).ConvertTo<T>(),
                Vector2Int => Math.Lerp(from.ConvertTo<Vector2Int>(), to.ConvertTo<Vector2Int>(), t, power)
                    .ConvertTo<T>(),
                Vector3Int => Math.Lerp(from.ConvertTo<Vector3Int>(), to.ConvertTo<Vector3Int>(), t, power)
                    .ConvertTo<T>(),
                Color => Math.Lerp(from.ConvertTo<Color>(), to.ConvertTo<Color>(), t, power).ConvertTo<T>(),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Normalize01<T>(this T a, T min, T max)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => Math.Normalize01(a.ConvertTo<int>(), min.ConvertTo<int>(), max.ConvertTo<int>()).ConvertTo<T>(),
                float => Math.Normalize01(a.ConvertTo<float>(), min.ConvertTo<float>(), max.ConvertTo<float>())
                    .ConvertTo<T>(),
                double => Math.Normalize01(a.ConvertTo<double>(), min.ConvertTo<double>(), max.ConvertTo<double>())
                    .ConvertTo<T>(),
                Vector2 => Math.Normalize01(a.ConvertTo<Vector2>(), min.ConvertTo<Vector2>(), max.ConvertTo<Vector2>())
                    .ConvertTo<T>(),
                Vector3 => Math.Normalize01(a.ConvertTo<Vector3>(), min.ConvertTo<Vector3>(), max.ConvertTo<Vector3>())
                    .ConvertTo<T>(),
                Vector4 => Math.Normalize01(a.ConvertTo<Vector4>(), min.ConvertTo<Vector4>(), max.ConvertTo<Vector4>())
                    .ConvertTo<T>(),
                Vector2Int => Math.Normalize01(a.ConvertTo<Vector2Int>(), min.ConvertTo<Vector2Int>(),
                    max.ConvertTo<Vector2Int>()).ConvertTo<T>(),
                Vector3Int => Math.Normalize01(a.ConvertTo<Vector3Int>(), min.ConvertTo<Vector3Int>(),
                    max.ConvertTo<Vector3Int>()).ConvertTo<T>(),
                Color => throw new ArgumentException(),
                _ => throw new ArgumentException()
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T PointSymmetric<T>(this T a, T point)
            where T : IEquatable<T>
        {
            return a switch
            {
                int => Math.PointSymmetric(a.ConvertTo<int>(), point.ConvertTo<int>()).ConvertTo<T>(),
                float => Math.PointSymmetric(a.ConvertTo<float>(), point.ConvertTo<float>()).ConvertTo<T>(),
                double => Math.PointSymmetric(a.ConvertTo<double>(), point.ConvertTo<double>()).ConvertTo<T>(),
                Vector2 => Math.PointSymmetric(a.ConvertTo<Vector2>(), point.ConvertTo<Vector2>()).ConvertTo<T>(),
                Vector3 => Math.PointSymmetric(a.ConvertTo<Vector3>(), point.ConvertTo<Vector3>()).ConvertTo<T>(),
                Vector4 => Math.PointSymmetric(a.ConvertTo<Vector4>(), point.ConvertTo<Vector4>()).ConvertTo<T>(),
                Vector2Int => Math.PointSymmetric(a.ConvertTo<Vector2Int>(), point.ConvertTo<Vector2Int>())
                    .ConvertTo<T>(),
                Vector3Int => Math.PointSymmetric(a.ConvertTo<Vector3Int>(), point.ConvertTo<Vector3Int>())
                    .ConvertTo<T>(),
                Color => throw new ArgumentException(),
                _ => throw new ArgumentException()
            };
        }
    }
}