using UnityEngine.Tilemaps;
using VMFramework.Tools;

namespace VMFramework.Maps
{
    public sealed class TilemapPrefabController : PrefabController<Tilemap>
    {
        public override void SetPrefab(Tilemap newPrefab)
        {
            base.SetPrefab(newPrefab);
            
            Prefab.ClearAllTiles();
        }
    }
}