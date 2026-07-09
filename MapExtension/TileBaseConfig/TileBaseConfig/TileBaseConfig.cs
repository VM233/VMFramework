using UnityEngine.Tilemaps;
using VMFramework.GameLogicArchitecture;

namespace VMFramework.Maps 
{
    public abstract partial class TileBaseConfig : GamePrefab, ITileBaseConfig
    {
        public override string IDSuffix => "tile_base";

        #region Interface Implementation

        public abstract TileBase GetTileBase();

        #endregion
    }
}