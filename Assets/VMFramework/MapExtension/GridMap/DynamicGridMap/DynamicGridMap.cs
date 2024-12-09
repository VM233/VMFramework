using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ResourceManagement.Exceptions;
using VMFramework.Core;
using VMFramework.Core.Pools;
using VMFramework.GameEvents;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Maps
{
    public abstract partial class DynamicGridMap : IGridMap
    {
        private readonly INormalPool<IGridChunk> chunkPool;
        
        public readonly DynamicGridMapConfig config;

        private readonly Dictionary<Vector3Int, IGridChunk> chunks = new();

        public Vector3Int ChunkSize => config.chunkSize;
        
        public IReadOnlyParameterizedGameEvent<IGridChunk> ChunkCreatedEvent => chunkCreatedEvent;
        public IReadOnlyParameterizedGameEvent<IGridChunk> ChunkDestructedEvent => chunkDestructedEvent;

        private readonly GridChunkChangedEvent chunkCreatedEvent;
        private readonly GridChunkChangedEvent chunkDestructedEvent;

        #region Constructors

        protected DynamicGridMap(DynamicGridMapConfig config, DynamicGridMapInitializationInfo info)
        {
            config.chunkSize.AssertIsAllNumberAbove(0, nameof(config.chunkSize));
            
            this.config = config;

            if (info != null)
            {
                chunkPool = info.ChunkPool;
            }

            chunkPool ??= CreateDefaultChunkPool();

            chunkCreatedEvent = GameItemManager.Get<GridChunkChangedEvent>(GridChunkChangedEventConfig.ID);
            chunkDestructedEvent = GameItemManager.Get<GridChunkChangedEvent>(GridChunkChangedEventConfig.ID);
        }

        #endregion

        protected virtual INormalPool<IGridChunk> CreateDefaultChunkPool()
        {
            return new CreatablePoolItemsPool<IGridChunk, IGridMap>(this, gridMap => new GridChunk(), 70);
        }

        #region Chunk

        #region Creations and Destructions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryCreateChunk(Vector3Int position, out IGridChunk chunk)
        {
            if (config.chunkBounds.Contains(position) == false)
            {
                Debugger.LogError($"Position {position} is outside the chunk bounds: {config.chunkBounds}. " +
                                  $"Cannot create new chunk.");
                chunk = null;
                return false;
            }
            
            if (chunks.ContainsKey(position))
            {
                Debugger.LogError($"Chunk at position {position} already exists. Cannot create new chunk.");
                chunk = null;
                return false;
            }
            
            chunk = chunkPool.Get();
            chunk.Place(new(position));
            chunks[position] = chunk;
            
            chunkCreatedEvent.Propagate(chunk);
            
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestructChunk(Vector3Int position)
        {
            if (chunks.TryGetValue(position, out var chunk) == false)
            {
                throw new OperationException($"Chunk at position {position} does not exist. Cannot destroy chunk.");
            }
            
            chunkDestructedEvent.Propagate(chunk);
            
            chunkPool.Return(chunk);
            
            chunks.Remove(position);
        }

        #endregion

        #region Query

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasValidChunk(Vector3Int position)
        {
            return chunks.ContainsKey(position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetChunk(Vector3Int chunkPosition, out IGridChunk chunk)
        {
            return chunks.TryGetValue(chunkPosition, out chunk);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IGridChunk GetChunk(Vector3Int chunkPosition)
        {
            return chunks.GetValueOrDefault(chunkPosition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<IGridChunk> GetAllChunks() => chunks.Values;

        #endregion

        #endregion

        #region Tile

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<Vector3Int> GetAllPoints()
        {
            foreach (var tile in GetAllTiles())
            {
                yield return tile.Position;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<IGridTile> GetAllTiles()
        {
            foreach (var chunk in chunks.Values)
            {
                foreach (var tile in chunk.GetAllTiles())
                {
                    yield return tile;
                }
            }
        }

        #endregion
        
        public virtual void ClearMap()
        {
            foreach (var chunk in chunks.Values)
            {
                chunk.ClearMap();
            }
        }
    }
}