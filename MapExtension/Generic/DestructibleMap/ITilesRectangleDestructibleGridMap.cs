using VMFramework.Core;

namespace VMFramework.Maps
{
    public interface ITilesRectangleDestructibleGridMap
    {
        public void DestructRectangleTiles(RectangleInteger rectangle);
    }
    
    public interface ITilesRectangleDestructibleGridMap<in TInfo> : ITilesRectangleDestructibleGridMap
    {
        public void DestructRectangleTiles(RectangleInteger rectangle, TInfo info);

        void ITilesRectangleDestructibleGridMap.DestructRectangleTiles(RectangleInteger rectangle)
        {
            DestructRectangleTiles(rectangle, default);
        }
    }
}