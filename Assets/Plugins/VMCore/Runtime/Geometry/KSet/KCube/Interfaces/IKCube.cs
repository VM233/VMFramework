using System;
using System.Runtime.CompilerServices;

namespace VMFramework.Core
{
    /// <summary>
    /// K维立方体接口
    /// </summary>
    /// <typeparam name="TPoint">K维立方体的点类型</typeparam>
    public interface IKCube<TPoint> : IKSet<TPoint>, IMinMaxOwner<TPoint>, IChooser<TPoint>
        where TPoint : struct, IEquatable<TPoint>
    {
        /// <summary>
        /// K维立方体的大小
        /// </summary>
        public TPoint Size { get; }

        /// <summary>
        /// K维立方体的中心点
        /// </summary>
        public TPoint Pivot { get; }

        /// <summary>
        /// 确保这个点比K维立方体的最小点大
        /// </summary>
        public TPoint ClampMin(TPoint pos);

        /// <summary>
        /// 确保这个点比K维立方体的最大点小
        /// </summary>
        public TPoint ClampMax(TPoint pos);
        
        /// <summary>
        /// 返回一个点相对于K维立方体的位置
        /// </summary>
        public TPoint GetRelativePos(TPoint pos);

        void IChooser.ResetChooser()
        {
            // Do nothing
        }
    }
}
