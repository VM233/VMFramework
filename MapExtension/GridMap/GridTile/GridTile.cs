using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.Core;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Maps
{
    public abstract class GridTile : ControllerGameItem, IGridTile
    {
        public IGridChunk Chunk { get; private set; }

        [ShowInInspector, DisplayAsString]
        public Vector3Int Position { get; private set; }

        public Vector2Int PositionXY => Position.XY();
        
        [ShowInInspector, DisplayAsString]
        public Vector3Int PositionInChunk { get; private set; }

        public virtual void InitGridTileInfo(GridTilePlaceInfo info)
        {
            Chunk = info.chunk;
            Position = info.position;
            PositionInChunk = info.positionInChunk;
        }

        protected override void OnGetStringProperties(ICollection<(string propertyID, string propertyContent)> collection)
        {
            base.OnGetStringProperties(collection);

            collection.Add(("pos", Position.ToString()));
        }
    }
}