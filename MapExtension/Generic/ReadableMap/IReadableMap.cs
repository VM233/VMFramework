using System.Collections.Generic;
using VMFramework.Core;

namespace VMFramework.Maps
{
    public interface IReadableMap<TPoint>
    {
        public IEnumerable<TPoint> GetAllPoints();
        
        public bool ContainsTile(TPoint point);
    }
    
    public interface IReadableMap<TPoint, TTile> : IReadableMap<TPoint>, IMapping<TPoint, TTile>
    {
        public IEnumerable<TTile> GetAllTiles();
        
        public IEnumerable<(TPoint, TTile)> GetAllTilesWithPoints();

        public TTile GetTile(TPoint point);
        
        public bool TryGetTile(TPoint point, out TTile tile)
        {
            tile = GetTile(point);
            return tile != null;
        }

        bool IReadableMap<TPoint>.ContainsTile(TPoint point)
        {
            return GetTile(point) != null;
        }

        TTile IMapping<TPoint, TTile>.MapTo(TPoint point) => GetTile(point);
    }
}