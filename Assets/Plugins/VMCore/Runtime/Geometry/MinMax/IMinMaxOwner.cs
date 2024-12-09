using System;

namespace VMFramework.Core
{
    public interface IMinMaxOwner<TPoint> where TPoint : struct, IEquatable<TPoint>
    {
        public TPoint Min { get; init; }
        
        public TPoint Max { get; init; }
    }
}