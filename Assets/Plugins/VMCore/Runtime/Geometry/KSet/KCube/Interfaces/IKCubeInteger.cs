using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    /// <summary>
    /// K维整数立方体接口
    /// </summary>
    /// <typeparam name="TPoint"></typeparam>
    public interface IKCubeInteger<TPoint> : IKCube<TPoint>, IEnumerableKSet<TPoint>
        where TPoint : struct, IEquatable<TPoint>
    {

    }
}