using VMFramework.Core;

namespace VMFramework.Maps
{
    public interface ITilesRectangleReplaceableGridMap<in TTileInfo>
    {
        public void ReplaceRectangleTiles(RectangleInteger rectangle, TTileInfo tileInfo);
    }
}