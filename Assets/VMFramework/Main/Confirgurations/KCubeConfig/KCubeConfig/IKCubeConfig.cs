using System;
using VMFramework.Core;

namespace VMFramework.Configuration
{
    public interface IKCubeConfig<TPoint> : IKCube<TPoint>
        where TPoint : struct, IEquatable<TPoint>
    {
        public new TPoint Min { get; set; }

        public new TPoint Max { get; set; }

        TPoint IMinMaxOwner<TPoint>.Min
        {
            get => Min;
            init => Min = value;
        }

        TPoint IMinMaxOwner<TPoint>.Max
        {
            get => Max;
            init => Max = value;
        }
    }
}