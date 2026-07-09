using System.Diagnostics.CodeAnalysis;
using VMFramework.Core;

namespace VMFramework.Maps
{
    public interface ITilesRectangleFillableGridMap<in TTileInfo>
    {
        public void FillRectangleTiles(RectangleInteger rectangle, [NotNull] TTileInfo tileInfo);
    }
}