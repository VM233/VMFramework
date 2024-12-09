using System;

namespace VMFramework.Core
{
    public interface ISteppedRange<TPoint> : IEnumerableKSet<TPoint>, IMinMaxOwner<TPoint>, IRandomItemProvider<TPoint>
        where TPoint : struct, IEquatable<TPoint>
    {
        public TPoint Step { get; }
    }
}