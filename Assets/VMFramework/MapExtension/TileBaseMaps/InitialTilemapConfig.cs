using Sirenix.OdinInspector;
using UnityEngine.Tilemaps;
using VMFramework.Configuration;

namespace VMFramework.Maps
{
    public sealed class InitialTilemapConfig : BaseConfig
    {
        public int layer;
        
        [Required]
        public Tilemap tilemap;
    }
}