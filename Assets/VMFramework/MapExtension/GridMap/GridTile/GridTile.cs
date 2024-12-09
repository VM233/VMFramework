using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Maps
{
    public abstract class GridTile : GeneralVisualGameItem, IGridTile
    {
        private IGridChunk chunk;

        IGridChunk IGridTile.Chunk => chunk;
        
        public IReadOnlyGridChunk Chunk => chunk;

        public Vector3Int Position { get; private set; }
        
        public Vector3Int PositionInChunk { get; private set; }

        public virtual void InitGridTileInfo(GridTilePlaceInfo info)
        {
            chunk = info.chunk;
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