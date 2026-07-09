using VMFramework.Core.Pools;

namespace VMFramework.Maps
{
    public class DynamicGridMapInitializationInfo
    {
        public INormalPool<IGridChunk> ChunkPool { get; init; }
    }
}