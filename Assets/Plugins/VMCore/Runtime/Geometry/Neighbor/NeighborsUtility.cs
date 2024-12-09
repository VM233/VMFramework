using System;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    public static class NeighborsUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static LeftRightNeighbors<TTo> Map<TFrom, TTo>(this LeftRightNeighbors<TFrom> from,
            Func<TFrom, TTo> mapFunc)
        {
            return new LeftRightNeighbors<TTo>(mapFunc(from.left), mapFunc(from.right));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FourDirectionsNeighbors<TTo> Map<TFrom, TTo>(this FourDirectionsNeighbors<TFrom> from,
            Func<TFrom, TTo> mapFunc)
        {
            return new FourDirectionsNeighbors<TTo>(mapFunc(from.left), mapFunc(from.right), mapFunc(from.up),
                mapFunc(from.down));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static EightDirectionsNeighbors<TTo> Map<TFrom, TTo>(this EightDirectionsNeighbors<TFrom> from,
            Func<TFrom, TTo> mapFunc)
        {
            return new EightDirectionsNeighbors<TTo>(mapFunc(from.left), mapFunc(from.right), mapFunc(from.up),
                mapFunc(from.down), mapFunc(from.upLeft), mapFunc(from.upRight), mapFunc(from.downLeft),
                mapFunc(from.downRight));
        }
    }
}