using Sirenix.OdinInspector;
using UnityEngine.Tilemaps;

namespace VMFramework.Maps
{
    public sealed partial class DefaultTileBaseConfig : TileBaseConfig
    {
        [TabGroup(TAB_GROUP_NAME, BASIC_CATEGORY)]
        [Required]
        public TileBase tile;

        public override TileBase GetTileBase() => tile;
    }
}