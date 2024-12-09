namespace VMFramework.Maps
{
    public interface ITileReplaceableMap<in TPoint, in TTileInfo>
    {
        /// <summary>
        /// Replaces the tile at the given position with the given tile info.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="info"></param>
        /// <returns>True if a tile was destructed, false otherwise.</returns>
        public bool ReplaceTile(TPoint position, TTileInfo info);
    }
}