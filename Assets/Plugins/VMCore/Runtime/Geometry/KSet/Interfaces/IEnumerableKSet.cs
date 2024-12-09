using System;
using System.Collections.Generic;

namespace VMFramework.Core
{
    public interface IEnumerableKSet<TPoint> : IKSet<TPoint>, IReadOnlyCollection<TPoint>
        where TPoint : struct, IEquatable<TPoint>
    {
        
    }
}