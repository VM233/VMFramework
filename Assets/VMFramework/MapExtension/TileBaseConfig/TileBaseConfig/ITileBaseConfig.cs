using UnityEngine.Tilemaps;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Maps 
{
    public interface ITileBaseConfig : IGamePrefab
    {
        public TileBase GetTileBase();
    }
}