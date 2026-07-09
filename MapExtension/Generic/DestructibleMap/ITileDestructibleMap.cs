namespace VMFramework.Maps
{
    public interface ITileDestructibleMap<in TPoint, TTile>
    {
        public bool DestructTile(TPoint position, out TTile tile);
    }
    
    public interface ITileDestructibleMap<in TPoint, in TInfo, TTile> : ITileDestructibleMap<TPoint, TTile>
    {
        public bool DestructTile(TPoint position, TInfo info, out TTile tile);

        bool ITileDestructibleMap<TPoint, TTile>.DestructTile(TPoint position, out TTile tile)
        {
            return DestructTile(position, default, out tile);
        }
    }
}