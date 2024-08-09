﻿using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VMFramework.Core;

namespace VMFramework.Maps
{
    public interface IReadableMap<in TPoint>
    {
        public bool ContainsTile(TPoint point);
    }
    
    public interface IReadableMap<TPoint, TTile> : IReadableMap<TPoint>, IMapping<TPoint, TTile>
    {
        public IEnumerable<TPoint> GetAllPoints();

        public IEnumerable<TTile> GetAllTiles();

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